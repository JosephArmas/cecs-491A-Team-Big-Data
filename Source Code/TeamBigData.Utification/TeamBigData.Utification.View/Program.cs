using System.Security.Principal;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.View.Abstraction;
using TeamBigData.Utification.View.Views;

namespace TeamBigData.Utification // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var response = new Response();
            UserAccount userAccount = new UserAccount("","");
            UserProfile userProfile = new UserProfile("");
            IView menu = new AnonymousView();
            while (true)
            {
                if (((IPrincipal)userProfile).IsInRole("Anonymous User"))
                {
                    menu = new AnonymousView();
                    response = menu.DisplayMenu(ref userAccount, ref userProfile);
                    if (!response.isSuccessful)
                    {
                        Console.Clear();
                        Console.WriteLine(response.errorMessage);
                        return;
                    }
                }
                else if (((IPrincipal)userProfile).IsInRole("Regular User"))
                {
                    menu = new RegularView();
                    response = menu.DisplayMenu(ref userAccount, ref userProfile);
                    if (!response.isSuccessful)
                    {
                        Console.Clear();
                        Console.WriteLine(response.errorMessage);
                        return;
                    }
                }
                else if (((IPrincipal)userProfile).IsInRole("Admin User"))
                {
                    menu = new AdminView();
                    response = menu.DisplayMenu(ref userAccount, ref userProfile);
                    if (!response.isSuccessful)
                    {
                        Console.Clear();
                        Console.WriteLine(response.errorMessage);
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Error Bad Role: " + userProfile.Identity.Name);
                    break;
                }
            }
        }
    }
}