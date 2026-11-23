using EntityFrameWork04.TransactionType;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameWork04.Data
{
    internal class BankContext : DbContext
    {

        public DbSet<Branch> Branches => Set<Branch>();
        public DbSet<Manager> Managers => Set<Manager>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<CustomerAccount> CustomerAccounts => Set<CustomerAccount>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(
                "Server=.;Database=NationalBankDb;Trusted_Connection=True;TrustServerCertificate=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BankContext).Assembly);

        }
    }
}