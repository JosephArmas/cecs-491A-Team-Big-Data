using System;

namespace MainMenu
{
    public static class Menu
    {
        /// <summary>
        /// Display all startup options.
        /// </summary>
        public static void displayMenu()
        {
            Console.WriteLine("[1] Login");
            Console.WriteLine("[2] Logout");
            Console.WriteLine("[3] Create a pin");
            Console.WriteLine("[4] Create a Service");
            Console.WriteLine("[5] Request a Service");
            Console.WriteLine("[6] ");
            Console.WriteLine("[7] ");
            Console.WriteLine("[8] ");
            Console.WriteLine("[9] Clear the logs");
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

        /// <summary>
        /// Display search options: name or id.
        /// </summary>
        public static void displaySearchOptions()
        {
            Console.WriteLine("[1] find by name");
            Console.WriteLine("[2] find by id");
        }
    }
}
