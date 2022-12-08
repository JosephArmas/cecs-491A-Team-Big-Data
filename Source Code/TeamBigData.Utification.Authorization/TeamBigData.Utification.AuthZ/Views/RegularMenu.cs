using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.Authorization.Views
{
    public static class RegularMenu
    {
        /// <summary>
        /// Display all startup options.
        /// </summary>
        public static void displayMenu()
        {
            Console.WriteLine("Welcome Regular User");
            Console.WriteLine("---------MENU---------");
            Console.WriteLine("[1] LogOut");
            Console.WriteLine("[0] exit");
            Console.WriteLine("Enter 0-1");
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
