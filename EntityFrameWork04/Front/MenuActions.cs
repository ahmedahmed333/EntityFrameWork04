using EntityFrameWork04.Data;
using EntityFrameWork04.Models.Enums;
using EntityFrameWork04.TransactionType;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameWork04.Front
{
    internal class MenuActions
    {
        public static void AddCustomer()
        {
            Console.WriteLine("--- Add New Customer ---");
            using var db = new BankContext();

            var fullName = ConsoleHelper.ReadRequired("Full Name", 150);

            string nationalId;
            while (true)
            {
                nationalId = ConsoleHelper.ReadNationalId("National ID");
                if (!db.Customers.Any(c => c.NationalId == nationalId)) break;
                ConsoleHelper.Error("  A customer with this National ID already exists.");
            }

            var dateOfBirth = ConsoleHelper.ReadDate("Date of Birth");
            var email = ConsoleHelper.ReadEmail("Email", 200);
            var phone = ConsoleHelper.ReadPhone("Phone");
            var address = ConsoleHelper.ReadRequired("Address", 250);
            var type = ConsoleHelper.ReadEnum<OwnershipType>("Customer Type");

            var customer = new Customer
            {
                FullName = fullName,
                NationalId = nationalId,
                DateOfBirth = dateOfBirth,
                Email = email,
                PhoneNumber = phone,
                Address = address,
                CustomerType = type
            };

            db.Customers.Add(customer);
            db.SaveChanges();

            Console.WriteLine();
            ConsoleHelper.Success($"Customer created successfully. CustomerId = {customer.Id}");
        }

        // =====================================================================
        // 2) Open a new Account for a Customer
        // =====================================================================
        public static void OpenAccount()
        {
            Console.WriteLine("--- Open New Account ---");

            var accountNumber = ConsoleHelper.ReadRequired("Account Number", 20);
            var accountType = ConsoleHelper.ReadEnum<AccountType>("Account Type");
            var branchCode = ConsoleHelper.ReadRequired("Branch Code", 20);
            var customerId = ConsoleHelper.ReadInt("Customer Id");
            var ownership = ConsoleHelper.ReadEnum<OwnershipType>("Ownership Role");

            using var db = new BankContext();
            ConsoleHelper.Info($"Validating branch '{branchCode}' and customer #{customerId}...");

            var branch = db.Branches.Find(branchCode);
            if (branch is null)
            {
                ConsoleHelper.Error($"Branch '{branchCode}' does not exist.");
                return;
            }

            if (!db.Customers.Any(c => c.Id == customerId))
            {
                ConsoleHelper.Error($"Customer #{customerId} does not exist.");
                return;
            }

            var account = db.Accounts.Find(accountNumber);

            if (account is null)
            {
                if (ownership != OwnershipType.Primary)
                {
                    ConsoleHelper.Error("A new account must start with a Primary owner.");
                    return;
                }

                account = new Account
                {
                    AccountNumber = accountNumber,
                    AccountType = accountType,
                    BranchCode = branch.Code,
                    OpeningDate = DateTime.Now,
                    CurrentBalance = 0
                };
                db.Accounts.Add(account);
            }
            else
            {
                if (!string.Equals(account.BranchCode, branch.Code, StringComparison.OrdinalIgnoreCase))
                {
                    ConsoleHelper.Error($"Account '{accountNumber}' belongs to branch '{account.BranchCode}', not '{branch.Code}'.");
                    return;
                }

                if (ownership == OwnershipType.Primary)
                {
                    ConsoleHelper.Error($"Account '{accountNumber}' already exists. Use CoHolder to add another owner.");
                    return;
                }

                if (db.CustomerAccounts.Any(ca => ca.AccountNumber == account.AccountNumber && ca.CustomerId == customerId))
                {
                    ConsoleHelper.Error($"Customer #{customerId} is already linked to account '{accountNumber}'.");
                    return;
                }
            }

            db.CustomerAccounts.Add(new CustomerAccount
            {
                CustomerId = customerId,
                AccountNumber = account.AccountNumber,
                OwnershipType = ownership,
                OwnershipStartDate = DateTime.Now,
                AccountStatus = AccountStatus.Active
            });
            db.SaveChanges();

            ConsoleHelper.Success($"Account '{account.AccountNumber}' created and linked to customer {customerId} as {ownership} owner.");
        }

        // =====================================================================
        // 3) Update Account Status  (الـ Status على الـ CustomerAccount مش على Account)
        // =====================================================================
        public static void UpdateAccountStatus()
        {
            Console.WriteLine("--- Update Account Status ---");

            var accountNumber = ConsoleHelper.ReadRequired("Account Number", 20);
            var customerId = ConsoleHelper.ReadInt("Customer Id");
            var newStatus = ConsoleHelper.ReadEnum<AccountStatus>("New Status");

            using var db = new BankContext();

            var link = db.CustomerAccounts.Find(customerId, accountNumber);   // نفس ترتيب HasKey
            if (link is null)
            {
                ConsoleHelper.Error($"Customer #{customerId} is not linked to account '{accountNumber}'.");
                return;
            }

            if (link.AccountStatus == newStatus)
            {
                ConsoleHelper.Info($"Status is already {newStatus}. Nothing to change.");
                return;
            }

            link.AccountStatus = newStatus;
            db.SaveChanges();

            ConsoleHelper.Success($"Status updated to {newStatus}.");
        }

        // =====================================================================
        // 4) Remove an Account from a Customer
        // =====================================================================
        public static void RemoveAccountFromCustomer()
        {
            Console.WriteLine("--- Remove Account From Customer ---");

            var accountNumber = ConsoleHelper.ReadRequired("Account Number", 20);
            var customerId = ConsoleHelper.ReadInt("Customer Id");

            using var db = new BankContext();

            var link = db.CustomerAccounts.Find(customerId, accountNumber);
            if (link is null)
            {
                ConsoleHelper.Error($"Customer #{customerId} is not linked to account '{accountNumber}'.");
                return;
            }

            var isLastOwner = !db.CustomerAccounts
                .Any(ca => ca.AccountNumber == link.AccountNumber && ca.CustomerId != customerId);

            Account? account = null;
            if (isLastOwner)
            {
                account = db.Accounts.Find(link.AccountNumber)!;

                if (account.CurrentBalance != 0 || db.Transactions.Any(t => t.AccountNumber == account.AccountNumber))
                {
                    ConsoleHelper.Error("Cannot remove the last owner: the account has a balance or transaction history. Close it instead (option 3).");
                    return;
                }
            }

            db.CustomerAccounts.Remove(link);
            if (account is not null) db.Accounts.Remove(account);
            db.SaveChanges();

            Console.WriteLine();
            ConsoleHelper.Success("  Ownership link deleted.");
            if (account is not null)
                ConsoleHelper.Info($"      That was the last owner - account '{account.AccountNumber}' was also removed.");
        }

        // =====================================================================
        // 5) List all Customers (Eager Loading)
        // =====================================================================
        public static void ListCustomers()
        {
            Console.WriteLine("--- All Customers ---");
            Console.WriteLine();

            using var db = new BankContext();

            var customers = db.Customers
                .AsNoTracking()                                 
                .Include(c => c.CustomerAccounts)
                    .ThenInclude(ca => ca.Account)
                        .ThenInclude(a => a.Branch)
                .OrderBy(c => c.Id)
                .ToList();

            if (customers.Count == 0)
            {
                Console.WriteLine("  (no customers yet)");
                return;
            }

            foreach (var c in customers)
            {
                Console.WriteLine($"  #{c.Id} {c.FullName} ({c.CustomerType})");

                if (c.CustomerAccounts.Count == 0)
                {
                    Console.WriteLine("      (no accounts)");
                    continue;
                }

                foreach (var ca in c.CustomerAccounts.OrderBy(x => x.AccountNumber))
                {
                    var a = ca.Account;
                    Console.WriteLine(
                        $"      {a.AccountNumber,-10} {a.AccountType,-9} Balance: {a.CurrentBalance,12:N2}   " +
                        $"{ca.OwnershipType,-9} {ca.AccountStatus,-7} @ {a.Branch.Name}");
                }
            }
        }
    }
}
