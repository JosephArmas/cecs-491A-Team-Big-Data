using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.View.Abstraction;

namespace TeamBigData.Utification.View.Views
{
    public class AnonymousView : IView
    {
        /// <summary>
        /// Display all startup options.
        /// </summary>
        public Response DisplayMenu(ref UserAccount userAccount, ref UserProfile userProfile)
        {
            Response response = new Response();
            if (!(((IPrincipal)userProfile).IsInRole("Anonymous User")))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to view";
                return response;
            }
            Console.WriteLine("Welcome Anonymouse User");
            Console.WriteLine("------------MENU------------");
            Console.WriteLine("[1] Create a New Account");
            Console.WriteLine("[2] Login");
            Console.WriteLine("[0] exit");
            Console.WriteLine("Enter 0-2");
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
                    break;
                case 2:
                    break;
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
