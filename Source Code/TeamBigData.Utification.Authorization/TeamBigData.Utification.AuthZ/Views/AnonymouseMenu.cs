using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Authorization;

namespace TeamBigData.Utification.Authorization.Views
{
    public static class AnonymouseMenu 
    {
        /// <summary>
        /// Display all startup options.
        /// </summary>
        public static void displayMenu()
        {
            Console.WriteLine("Welcome Anonymouse User");
            Console.WriteLine("------------MENU------------");
            Console.WriteLine("[1] Create a New Account");
            Console.WriteLine("[2] Login");
            Console.WriteLine("[0] exit");
            Console.WriteLine("Enter 0-2");
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
