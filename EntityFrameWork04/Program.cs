using EntityFrameWork04.Front;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameWork04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                ConsoleHelper.PrintMenu();
                var choice = Console.ReadLine()?.Trim();

                if (choice == "0")
                {
                    Console.WriteLine();
                    Console.WriteLine("Goodbye!");
                    break;
                }

                Console.Clear();

                try
                {
                    switch (choice)
                    {
                        case "1": MenuActions.AddCustomer(); break;
                        case "2": MenuActions.OpenAccount(); break;
                        case "3": MenuActions.UpdateAccountStatus(); break;
                        case "4": MenuActions.RemoveAccountFromCustomer(); break;
                        case "5": MenuActions.ListCustomers(); break;
                        default:
                            ConsoleHelper.Error("Invalid choice. Please enter a number from the menu.");
                            break;
                    }
                }
                catch (DbUpdateException ex)
                {
                    ConsoleHelper.Error($"Database error: {ex.InnerException?.Message ?? ex.Message}");
                }
                catch (Exception ex)
                {
                    ConsoleHelper.Error($"Unexpected error: {ex.Message}");
                }

                ConsoleHelper.Pause();
            }
            Console.WriteLine("Hello, World!");
        }
    }
}
