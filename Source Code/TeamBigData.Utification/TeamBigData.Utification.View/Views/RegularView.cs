﻿using System.Security.Principal;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Manager.Abstractions;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.View.Abstraction;

namespace TeamBigData.Utification.View.Views
{
    public class RegularView : IView
    {
        /// <summary>
        /// Display all startup options.
        /// </summary>
        public Response DisplayMenu(ref UserAccount userAccount, ref UserProfile userProfile)
        {
            Response response = new Response();
            ILogout logout = new SecurityManager();
            if (!((IPrincipal)userProfile).IsInRole("Regular User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to view";
                return response;
            }
            Console.Clear(); 
            Console.WriteLine("\nWelcome " + userAccount._username);
            Console.WriteLine("Regular User View");
            Console.WriteLine("---------MENU---------");
            Console.WriteLine("[1] LogOut");
            Console.WriteLine("[0] exit");
            Console.Write("Enter 0-1: ");
            string input = Console.ReadLine();
            switch (input)
            {
                case "0":
                    response.isSuccessful = false;
                    response.errorMessage = "";
                    return response;
                case "1":
                    logout.LogOutUser(ref userAccount, ref userProfile);
                    Console.WriteLine("\nSuccessfully logged out");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("Invalid Input\nPress Enter to Try Again...");
                    Console.ReadLine();
                    break;
            }
            response.isSuccessful = true;
            response.errorMessage = "";
            return response;
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
