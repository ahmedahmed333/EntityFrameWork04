using System;
using System.Collections.Generic;
using System.Text;

using EntityFrameWork04.Models.Enums;
using OwnershipTypeEnum = EntityFrameWork04.Models.Enums.OwnershipType;


namespace EntityFrameWork04.TransactionType
{
    internal class Customer
    {

        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public OwnershipTypeEnum CustomerType { get; set; }

        public ICollection<CustomerAccount> CustomerAccounts { get; set; } = new List<CustomerAccount>();
    }
}
