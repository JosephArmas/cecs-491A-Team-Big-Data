using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Authorization;
using TeamBigData.Utification.AuthZ.Abstraction;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.Authorization.Views
{
    public class AnonymouseMenu : IMenu
    {
        /// <summary>
        /// Display all startup options.
        /// </summary>
        public bool DisplayMenu(ref UserAccount userAccount, ref UserProfile userProfile)
        {
            Console.WriteLine("Welcome Anonymouse User");
            Console.WriteLine("------------MENU------------");
            Console.WriteLine("[1] Create a New Account");
            Console.WriteLine("[2] Login");
            Console.WriteLine("[0] exit");
            Console.WriteLine("Enter 0-2");
            string input = Console.ReadLine();
            switch (Int32.Parse(input))
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("Exiting Utification...");
                    return false;
                case 1:
                    // Authorization test...
                    Console.Clear();
                    Console.WriteLine("Checking if user is a Regular User");
                    Console.WriteLine(((IPrincipal)userProfile).IsInRole("Regular User"));
                    Console.WriteLine("Checking if user is an Anonymous User");
                    Console.WriteLine(((IPrincipal)userProfile).IsInRole("Anonymous User"));
                    Console.ReadLine();
                    Console.Clear();
                    break;
                case 2:
                    break;
                default:
                    break;
            }
            return true;
        }

        /// <summary>
        /// Clear console and print seperator.
        /// </summary>
        public void ClearMenu()
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------");
        }
    }
}
