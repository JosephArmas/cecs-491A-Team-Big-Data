using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.AuthZ.Abstraction;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.Authorization.Views
{
    public class AdminMenu : IMenu
    {
        /// <summary>
        /// Display all startup options.
        /// </summary>
        public bool DisplayMenu(ref UserAccount userAccount, ref UserProfile userProfile)
        {
            Console.WriteLine("Welcome Admin User");
            Console.WriteLine("---------MENU---------");
            Console.WriteLine("[1] View All User Accounts");
            Console.WriteLine("[2] View All User Profiles");
            Console.WriteLine("[3] Re-enable User");
            Console.WriteLine("[4] LogOut");
            Console.WriteLine("[0] exit");
            Console.WriteLine("Enter 0-4");
            string input = Console.ReadLine();
            Response response = new Response();
            switch (Int32.Parse(input))
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("Logging Out User...\nExiting Utification...");
                    return false;
                case 1:
                    break;
                case 2:
                    Console.Clear();
                    SecurityManager secManager = new SecurityManager();
                    List<UserProfile> list = new List<UserProfile>();
                    response = secManager.GetUserProfileTable(list);
                    Console.WriteLine("Printing out User Profile Table");
                    for (int i = 0; i < list.Count; i++)
                        Console.WriteLine(((UserProfile)list[i]).ToString());
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
                case 3:
                    userAccount = new UserAccount();
                    userProfile = new UserProfile("Anonymous");
                    break;
                case 4:
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
