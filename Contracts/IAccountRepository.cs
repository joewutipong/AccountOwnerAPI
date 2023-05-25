using System;
using Entities.Models;

namespace Contracts
{
    public interface IAccountRepository : IRepositoryBase<Account>
    {
        Task<IEnumerable<Account>> AccountByOwner(Guid ownerId);
    }
}

