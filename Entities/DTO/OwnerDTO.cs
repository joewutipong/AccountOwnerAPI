using System;
namespace Entities.DTO
{
    public class OwnerDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }

        public IEnumerable<AccountDTO>? Accounts { get; set; }
    }
}

