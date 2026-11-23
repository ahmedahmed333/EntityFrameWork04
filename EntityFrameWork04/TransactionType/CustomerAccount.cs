using System;
using System.Collections.Generic;
using System.Text;
using EntityFrameWork04.Models.Enums;
namespace EntityFrameWork04.TransactionType
{
    internal class CustomerAccount
    {

        public int CustomerId { get; set; }
        public string AccountNumber { get; set; } = null!;
        public OwnershipType OwnershipType { get; set; }
        public DateTime OwnershipStartDate { get; set; }
        public AccountStatus AccountStatus { get; set; }

        public Customer Customer { get; set; } = null!;
        public Account Account { get; set; } = null!;
    }
}
