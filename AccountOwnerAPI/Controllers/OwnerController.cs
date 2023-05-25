using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DTO;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountOwnerAPI.Controllers
{
    [Route("api/owners")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public OwnerController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var owners = await _repository.Owner.GetAllOwnersAsync();

                _logger.LogInfo($"Returned all owners from database.");

                var ownersResult = _mapper.Map<IEnumerable<OwnerDTO>>(owners);

                return Ok(ownersResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllOwners action: {ex.Message}");

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "OwnerById")]
        public async Task<IActionResult> GetOwnerById(Guid id)
        {
            try
            {
                var owner = await _repository.Owner.GetOwnerByIdAsync(id);

                if (owner is null)
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _logger.LogInfo($"Returned owner with id: {id}");

                var ownerResult = _mapper.Map<OwnerDTO>(owner);
                return Ok(ownerResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOwnerById action: {ex.Message}");

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}/accounts")]
        public async Task<IActionResult> GetOwnerWithDetails(Guid id)
        {
            try
            {
                var owner = await _repository.Owner.GetOwnerWithDetailsAsync(id);

                if (owner is null)
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _logger.LogInfo($"Returned owner with details for id: {id}");

                var ownerResult = _mapper.Map<OwnerDTO>(owner);
                return Ok(ownerResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOwnerWithDetails action: {ex.Message}");

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOwner(OwnerForCreationDTO owner)
        {
            try
            {
                if (owner is null)
                {
                    _logger.LogError($"Owner object sent from client is null.");
                    return BadRequest("Owner object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError($"Invalid owner object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var ownerEntity = _mapper.Map<Owner>(owner);

                _repository.Owner.CreateOwner(ownerEntity);
                await _repository.SaveAsync();

                var createdOwner = _mapper.Map<OwnerDTO>(ownerEntity);

                return CreatedAtRoute("OwnerById", new { id = createdOwner.Id }, createdOwner);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateOwner action: {ex.Message}");

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOwner(Guid id, [FromBody] OwnerForUpdateDTO owner)
        {
            try
            {
                if (owner is null)
                {
                    _logger.LogError($"Owner object sent from client is null.");
                    return BadRequest("Owner object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError($"Invalid owner object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var ownerEntity = await _repository.Owner.GetOwnerByIdAsync(id);

                if (ownerEntity is null)
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(owner, ownerEntity);

                _repository.Owner.UpdateOwner(ownerEntity);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateOwner action: {ex.Message}");

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOwner(Guid id)
        {
            try
            {
                var owner = await _repository.Owner.GetOwnerByIdAsync(id);

                if (owner is null)
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                var accounts = await _repository.Account.AccountByOwner(id);

                if (accounts.Count() > 0)
                {
                    _logger.LogError($"Can't delete owner with id: {id}. It has related account. Delete those accounts first");
                    return BadRequest("Can't delete owner. It has related account. Delete those accounts first");
                }

                _repository.Owner.DeleteOwner(owner);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteOwner action: {ex.Message}");

                return StatusCode(500, "Internal server error");
            }
        }
    }
}

