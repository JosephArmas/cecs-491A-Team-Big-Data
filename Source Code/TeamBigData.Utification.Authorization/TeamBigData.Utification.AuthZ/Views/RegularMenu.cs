using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.AuthZ.Abstraction;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.Authorization.Views
{
    public class RegularMenu : IMenu
    {
        /// <summary>
        /// Display all startup options.
        /// </summary>
        public bool DisplayMenu(ref UserAccount userAccount, ref UserProfile userProfile)
        {
            Console.WriteLine("Welcome Regular User");
            Console.WriteLine("---------MENU---------");
            Console.WriteLine("[1] LogOut");
            Console.WriteLine("[0] exit");
            Console.WriteLine("Enter 0-1");
            string input = Console.ReadLine();
            switch (Int32.Parse(input))
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("Exiting Utification...");
                    return false;
                case 1:
                    userAccount = new UserAccount();
                    userProfile = new UserProfile("Anonymous");
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
