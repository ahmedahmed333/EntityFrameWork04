using EntityFrameWork04.TransactionType;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameWork04.Data.Configurations
{
    internal class CustomerAccountConfiguration : IEntityTypeConfiguration<CustomerAccount>

    {
        public void Configure(EntityTypeBuilder<CustomerAccount> builder)
        {
            builder.HasKey(ca => new { ca.CustomerId, ca.AccountNumber });

            builder.Property(ca => ca.AccountNumber).HasMaxLength(20);
            builder.Property(ca => ca.OwnershipType).HasConversion<string>().HasMaxLength(20);
            builder.Property(ca => ca.AccountStatus).HasConversion<string>().HasMaxLength(20);
            builder.Property(ca => ca.OwnershipStartDate).IsRequired();

            builder.HasOne(ca => ca.Customer)
                   .WithMany(c => c.CustomerAccounts)
                   .HasForeignKey(ca => ca.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ca => ca.Account)
                   .WithMany(a => a.CustomerAccounts)
                   .HasForeignKey(ca => ca.AccountNumber)
                   .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
