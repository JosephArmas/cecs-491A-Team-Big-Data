﻿using System.Security.Principal;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Manager;
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
            if (!((IPrincipal)userProfile).IsInRole("Regular User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to view";
                return response;
            }
            Console.WriteLine("Welcome Regular User");
            Console.WriteLine("---------MENU---------");
            Console.WriteLine("[2] Delete Account");
            Console.WriteLine("[1] LogOut");
            Console.WriteLine("[0] exit");
            Console.WriteLine("Enter 0-1");
            string input = Console.ReadLine();
            switch (Int32.Parse(input))
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("Exiting Utification...");
                    response.isSuccessful = false;
                    response.errorMessage = "";
                    return response;
                case 1:
                    userProfile = new UserProfile("");
                    Console.WriteLine("Successfully logged out");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
                case 2:
                    Console.WriteLine("Are you sure you wish to delete the account?(Y/N)");
                    var answer = Console.ReadLine();
                    if (answer == "N" || answer == "n")
                    {
                        break;
                    }
                    else if(answer =="Y" || answer =="y")
                    {
                        DeletionManager delManager = new DeletionManager();
                        response = delManager.DeleteAccount(userProfile.Identity.Name, userProfile);
                        if (!response.isSuccessful)
                        {
                            Console.Clear();
                            Console.WriteLine(response.errorMessage);
                            Console.WriteLine("Press Enter to exit...");
                            Console.ReadLine();
                            response.isSuccessful = false;
                            return response;
                        }
                        userProfile = new UserProfile("");
                        Console.WriteLine("Account Deletion Successful");
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid answer returning to menu");
                        break;
                    }
                default:
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
