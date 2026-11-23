using EntityFrameWork04.TransactionType;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameWork04.Data.Configurations
{
    internal class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.FullName).IsRequired().HasMaxLength(150);
            builder.Property(m => m.Email).IsRequired().HasMaxLength(200);
            builder.Property(m => m.PhoneNumber).IsRequired().HasMaxLength(20);
            builder.Property(m => m.HireDate).HasColumnType("date");

            builder.HasIndex(m => m.Email).IsUnique();
        }
    }
}
