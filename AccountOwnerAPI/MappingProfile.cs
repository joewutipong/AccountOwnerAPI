using System;
using AutoMapper;
using Entities.DTO;
using Entities.Models;

namespace AccountOwnerAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Owner, OwnerDTO>();
            CreateMap<Account, AccountDTO>();
            CreateMap<OwnerForCreationDTO, Owner>();
            CreateMap<OwnerForUpdateDTO, Owner>();
        }
    }
}

