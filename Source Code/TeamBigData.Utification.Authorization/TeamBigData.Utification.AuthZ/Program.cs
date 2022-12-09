using System;
using System.Security.Principal;
using TeamBigData.Utification.Authorization.Views;
using TeamBigData.Utification.AuthZ.Abstraction;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.Authorization // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input = "0";
            UserAccount userAccount = new UserAccount();
            UserProfile userProfile = new UserProfile("Anonymous User");
            IMenu menu = new AnonymouseMenu();  
            while (true)
            {
                if(((IPrincipal)userProfile).IsInRole("Anonymous User"))
                {
                    if (!menu.DisplayMenu(ref userAccount, ref userProfile))
                        return;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Unauthorized access to view");
                    return;
                }
                if (((IPrincipal)userProfile).IsInRole("Regular User"))
                {
                    if (!menu.DisplayMenu(ref userAccount, ref userProfile))
                        return;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Unauthorized access to view");
                    return;
                }
                if (((IPrincipal)userProfile).IsInRole("Admin User")) 
                {
                    if (!menu.DisplayMenu(ref userAccount, ref userProfile))
                        Console.WriteLine("Unauthorized access to view");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Unauthorized access to view");
                    return;
                }
            }
        }
    }
}