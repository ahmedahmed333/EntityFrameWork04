using EntityFrameWork04.TransactionType;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameWork04.Data
{
    internal static class SeedData
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manager>().HasData(
                new Manager { Id = 1, FullName = "Hassan Mahmoud", Email = "hassan.mahmoud@nationalbank.com", PhoneNumber = "01001112233", HireDate = new DateTime(2015, 3, 1) },
                new Manager { Id = 2, FullName = "Mariam Fawzy", Email = "mariam.fawzy@nationalbank.com", PhoneNumber = "01102223344", HireDate = new DateTime(2017, 9, 15) },
                new Manager { Id = 3, FullName = "Tarek Selim", Email = "tarek.selim@nationalbank.com", PhoneNumber = "01203334455", HireDate = new DateTime(2019, 1, 20) });

            modelBuilder.Entity<Branch>().HasData(
                new Branch { Code = "CAI-01", Name = "Cairo Main Branch", Address = "1 Tahrir Square, Cairo", PhoneNumber = "0225551001", ManagerId = 1 },
                new Branch { Code = "ALX-01", Name = "Alexandria Branch", Address = "25 Corniche Road, Alexandria", PhoneNumber = "0335552002", ManagerId = 2 },
                new Branch { Code = "GIZ-01", Name = "Giza Branch", Address = "10 Pyramids Street, Giza", PhoneNumber = "0235553003", ManagerId = 3 });
        }
    }
}
