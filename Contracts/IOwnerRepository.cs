using System;
using Entities.Models;

namespace Contracts
{
    public interface IOwnerRepository : IRepositoryBase<Owner>
    {
        Task<IEnumerable<Owner>> GetAllOwnersAsync();
        Task<Owner> GetOwnerByIdAsync(Guid ownerId);
        Task<Owner> GetOwnerWithDetailsAsync(Guid ownerId);
        void CreateOwner(Owner owner);
        void UpdateOwner(Owner owner);
        void DeleteOwner(Owner owner);
    }
}

