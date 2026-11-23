using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transaction = EntityFrameWork04.TransactionType.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameWork04.Data.Configurations
{
    internal class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.TransactionNumber);    

            builder.Property(t => t.TransactionDate).IsRequired();
            builder.Property(t => t.Amount).HasPrecision(18, 2);
            builder.Property(t => t.TransactionType).HasConversion<string>().HasMaxLength(20);
            builder.Property(t => t.Note).HasMaxLength(500);
            builder.Property(t => t.AccountNumber).IsRequired().HasMaxLength(20);

            builder.ToTable(t => t.HasCheckConstraint("CK_Transactions_Amount", "[Amount] > 0"));

            builder.HasOne(t => t.Account)
                   .WithMany(a => a.Transactions)
                   .HasForeignKey(t => t.AccountNumber)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
