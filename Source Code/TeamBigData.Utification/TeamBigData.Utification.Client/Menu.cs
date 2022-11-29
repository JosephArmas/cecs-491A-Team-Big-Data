using System;
//Adapted from CECS 475 Phuong Nguyen

namespace MainMenu
{
    public static class Menu
    {
        /// <summary>
        /// Display all startup options.
        /// </summary>
        public static void displayMenu()
        {
            Console.WriteLine("[1] Create a New Account");
            Console.WriteLine("[2] Login to an Account");
            Console.WriteLine("[3] Create a pin");
            Console.WriteLine("[0] exit");
        }

        /// <summary>
        /// Clear console and print seperator.
        /// </summary>
        public static void clearMenu()
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------");
        }
    }
}
