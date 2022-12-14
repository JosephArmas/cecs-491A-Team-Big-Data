using System;
using System.Security.Principal;
//using TeamBigData.Utification.Authorization.Views;
//using TeamBigData.Utification.AuthZ.Abstraction;
//using TeamBigData.Utification.ErrorResponse;
//using TeamBigData.Utification.Models;

namespace TeamBigData.Utification // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var response = new Response();
            UserAccount userAccount = new UserAccount();
            UserProfile userProfile = new UserProfile("Anonymous User");
            IView menu = new AnonymousView();
            while (true)
            {
                if (((IPrincipal)userProfile).IsInRole("Anonymous User"))
                {
                    response = menu.DisplayMenu(ref userAccount, ref userProfile);
                    if (!response.isSuccessful)
                    {
                        Console.Clear();
                        Console.WriteLine(response.errorMessage);
                        return;
                    }
                }
                if (((IPrincipal)userProfile).IsInRole("Regular User"))
                {
                    response = menu.DisplayMenu(ref userAccount, ref userProfile);
                    if (!response.isSuccessful)
                    {
                        Console.Clear();
                        Console.WriteLine(response.errorMessage);
                        return;
                    }
                }
                if (((IPrincipal)userProfile).IsInRole("Admin User"))
                {
                    response = menu.DisplayMenu(ref userAccount, ref userProfile);
                    if (!response.isSuccessful)
                    {
                        Console.Clear();
                        Console.WriteLine(response.errorMessage);
                        return;
                    }
                }
            }
        }
    }
}