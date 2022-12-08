using System;
using TeamBigData.Utification.Authorization.Views;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.Authorization // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            run();
        }
        public static void run()
        {
            string input = "0";
            UserAccount userAccount = new UserAccount();
            UserProfile userProfile = new UserProfile("Anonymous");

            while (true)
            {
                while (userProfile.IsInRole("Anonymous User"))
                {
                    AnonymouseMenu.displayMenu();
                    input = Console.ReadLine();
                    switch (Int32.Parse(input))
                    {
                        case 0:
                            Console.WriteLine("Exiting Utification...");
                            return;
                        case 1:
                            break;
                        case 2:
                            break;
                        default:
                            break;
                    }
                }
                while (userProfile.IsInRole("Regular User"))
                {
                    RegularMenu.displayMenu();
                    input = Console.ReadLine();
                    switch (Int32.Parse(input))
                    {
                        case 0:
                            Console.WriteLine("Exiting Utification...");
                            return;
                        case 1:
                            userAccount = new UserAccount();
                            userProfile = new UserProfile("Anonymous");
                            break;
                        default:
                            break;
                    }
                }
                while (userProfile.IsInRole("Admin User"))
                {
                    RegularMenu.displayMenu();
                    input = Console.ReadLine();
                    switch (Int32.Parse(input))
                    {
                        case 0:
                            Console.WriteLine("Logging Out User...\nExiting Utification...");
                            return;
                        case 1:

                            break;
                        case 2:
                            break;
                        case 3:
                            userAccount = new UserAccount();
                            userProfile = new UserProfile("Anonymous");
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}