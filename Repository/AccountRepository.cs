using System;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Account>> AccountByOwner(Guid ownerId)
        {
            return await FindByCondition(a => a.OwnerId.Equals(ownerId)).ToListAsync();
        }
    }
}

