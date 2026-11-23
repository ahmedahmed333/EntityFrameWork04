using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace EntityFrameWork04.Front
{
  
        public static class ConsoleHelper
        {
            public static void Success(string message) => Write(message, ConsoleColor.Green);
            public static void Error(string message) => Write(message, ConsoleColor.Red);
            public static void Info(string message) => Write(message, ConsoleColor.Cyan);

            private static void Write(string message, ConsoleColor color)
            {
                var old = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ForegroundColor = old;
            }

            public static void PrintMenu()
            {
                Console.WriteLine("========================================");
                Console.WriteLine("    National Bank - Management");
                Console.WriteLine("========================================");
                Console.WriteLine("  1) Add a new Customer");
                Console.WriteLine("  2) Open a new Account for a Customer");
                Console.WriteLine("  3) Update Account Status (Active / Closed)");
                Console.WriteLine("  4) Remove an Account from a Customer");
                Console.WriteLine("  5) List all Customers (with accounts)");
                Console.WriteLine("  0) Exit");
                Console.WriteLine("----------------------------------------");
                Console.Write("  Enter choice: ");
            }

            public static void Pause()
            {
                Console.WriteLine();
                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey(true);
            }

            public static string ReadRequired(string label, int maxLength)
            {
                while (true)
                {
                    Console.Write($"{label,-14}: ");
                    var value = Console.ReadLine()?.Trim();

                    if (string.IsNullOrWhiteSpace(value)) { Error($"  {label} is required."); continue; }
                    if (value.Length > maxLength) { Error($"  {label} must be at most {maxLength} characters."); continue; }
                    return value;
                }
            }

            public static int ReadInt(string label)
            {
                while (true)
                {
                    Console.Write($"{label,-14}: ");
                    if (int.TryParse(Console.ReadLine()?.Trim(), out var number) && number > 0)
                        return number;
                    Error("  Please enter a valid positive number.");
                }
            }

            public static DateTime ReadDate(string label)
            {
                while (true)
                {
                    Console.Write($"{label,-14}: (yyyy-MM-dd) ");
                    var raw = Console.ReadLine()?.Trim();

                    if (!DateTime.TryParseExact(raw, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                    { Error("  Use the format yyyy-MM-dd (e.g. 2000-01-25)."); continue; }

                    if (date > DateTime.Today || date.Year < 1900)
                    { Error("  That date of birth is not realistic."); continue; }

                    return date;
                }
            }

            public static string ReadEmail(string label, int maxLength)
            {
                while (true)
                {
                    var value = ReadRequired(label, maxLength);
                    try
                    {
                        if (new MailAddress(value).Address == value) return value;
                    }
                    catch (FormatException) { }
                    Error("  Invalid email format (e.g. name@mail.com).");
                }
            }

            public static string ReadPhone(string label)
            {
                while (true)
                {
                    var value = ReadRequired(label, 20);
                    if (Regex.IsMatch(value, @"^\+?\d{7,15}$")) return value;
                    Error("  Phone must contain 7-15 digits (optional leading +).");
                }
            }

            public static string ReadNationalId(string label)
            {
                while (true)
                {
                    var value = ReadRequired(label, 20);
                    if (Regex.IsMatch(value, @"^\d{10,20}$")) return value;
                    Error("  National ID must be 10-20 digits.");
                }
            }

            public static T ReadEnum<T>(string title) where T : struct, Enum
            {
                var values = Enum.GetValues<T>();
                while (true)
                {
                    Console.WriteLine($"{title}:");
                    for (var i = 0; i < values.Length; i++)
                        Console.WriteLine($"    {i + 1}) {values[i]}");

                    Console.Write("  Choice: ");
                    if (int.TryParse(Console.ReadLine()?.Trim(), out var choice) && choice >= 1 && choice <= values.Length)
                        return values[choice - 1];

                    Error("  Invalid choice, try again.");
                }
            }
        }
}
