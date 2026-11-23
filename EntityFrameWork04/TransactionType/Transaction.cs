using System;
using System.Collections.Generic;
using System.Text;

using EntityFrameWork04.Models.Enums;
using TransactionTypeEnum = EntityFrameWork04.Models.Enums.TransactionType;


namespace EntityFrameWork04.TransactionType
{
    internal class Transaction
    {

        public int TransactionNumber { get; set; }          
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public string? Note { get; set; }

        public string AccountNumber { get; set; } = null!;  
        public Account Account { get; set; } = null!;
    }
}
