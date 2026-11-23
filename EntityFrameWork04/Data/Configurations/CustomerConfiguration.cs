using EntityFrameWork04.TransactionType;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameWork04.Data.Configurations
{
    internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
    

        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FullName).IsRequired().HasMaxLength(150);
            builder.Property(c => c.NationalId).IsRequired().HasMaxLength(20);
            builder.Property(c => c.DateOfBirth).HasColumnType("date");
            builder.Property(c => c.Email).IsRequired().HasMaxLength(200);
            builder.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(20);
            builder.Property(c => c.Address).IsRequired().HasMaxLength(250);
            builder.Property(c => c.CustomerType).HasConversion<string>().HasMaxLength(20);

            builder.HasIndex(c => c.NationalId).IsUnique();
        }

    }
}
