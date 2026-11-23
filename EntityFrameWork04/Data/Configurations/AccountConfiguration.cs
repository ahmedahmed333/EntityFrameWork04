using EntityFrameWork04.TransactionType;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameWork04.Data.Configurations 
{
    internal class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => a.AccountNumber);

            builder.Property(a => a.AccountNumber).HasMaxLength(20);
            builder.Property(a => a.CurrentBalance).HasPrecision(18, 2);
            builder.Property(a => a.AccountType).HasConversion<string>().HasMaxLength(20);
            builder.Property(a => a.OpeningDate).IsRequired();
            builder.Property(a => a.BranchCode).IsRequired().HasMaxLength(20);

            builder.HasOne(a => a.Branch)
                   .WithMany(b => b.Accounts)
                   .HasForeignKey(a => a.BranchCode)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
