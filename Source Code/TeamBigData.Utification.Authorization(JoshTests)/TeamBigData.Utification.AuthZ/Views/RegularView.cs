using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.AuthZ.Abstraction;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.Authorization.Views
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
            Console.WriteLine("[1] LogOut");
            if (userProfile.ServiceProvider)
                Console.WriteLine("[?] Switch to ___ view");
            Console.WriteLine("[0] exit");
            Console.WriteLine("Enter 0-1");
            string input = Console.ReadLine();
            switch (Int32.Parse(input))
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("Exiting Utification...");
                    response.isSuccessful = false;
                    return response;
                case 1:
                    
                default:
                    break;
            }
            response.isSuccessful = true;
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
