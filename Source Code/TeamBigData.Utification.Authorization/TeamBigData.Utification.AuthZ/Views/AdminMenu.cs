using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.Authorization.Views
{
    public static class AdminMenu
    {
        /// <summary>
        /// Display all startup options.
        /// </summary>
        public static void displayMenu()
        {
            Console.WriteLine("Welcome Admin User");
            Console.WriteLine("---------MENU---------");
            Console.WriteLine("[1] View Disabled Users");
            Console.WriteLine("[2] Re-enable User");
            Console.WriteLine("[3] LogOut");
            Console.WriteLine("[0] exit");
            Console.WriteLine("Enter 0-4");
        }

        /// <summary>
        /// Clear console and print seperator.
        /// </summary>
        public static void ClearMenu()
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------");
        }
    }
}
