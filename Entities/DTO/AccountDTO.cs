﻿using System;
namespace Entities.DTO
{
    public class AccountDTO
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string? AccountType { get; set; }
    }
}

