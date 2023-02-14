﻿using System.Security.Principal;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.View.Abstraction;
using TeamBigData.Utification.View.Views;

namespace TeamBigData.Utification // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var response = new Response();
            UserAccount userAccount = new UserAccount("","","","");
            UserProfile userProfile = new UserProfile(0);
            IView menu = new AnonymousView();
            while (true)
            {
                if (((IPrincipal)userProfile).IsInRole("Anonymous User"))
                {
                    menu = new AnonymousView();
                    response = menu.DisplayMenu(ref userAccount, ref userProfile);
                    if (!response.isSuccessful && response.errorMessage == "")
                    {
                        Console.Clear();
                        Console.WriteLine(response.errorMessage + "\nPress Enter to Continue...");
                        Console.ReadLine();
                        return;
                    }
                    if (!response.isSuccessful && response.errorMessage != "")
                    {
                        Console.WriteLine(response.errorMessage + "\nPress Enter to Continue...");
                        Console.ReadLine();
                        return;
                    }
                }
                else if (((IPrincipal)userProfile).IsInRole("Regular User"))
                {
                    menu = new RegularView();
                    response = menu.DisplayMenu(ref userAccount, ref userProfile);
                    if (!response.isSuccessful && response.errorMessage == "")
                    {
                        Console.WriteLine(response.errorMessage + "\nPress Enter to Continue...");
                        Console.ReadLine();
                        return;
                    }
                    if (!response.isSuccessful && response.errorMessage != "")
                    {
                        Console.WriteLine(response.errorMessage + "\nPress Enter to Continue...");
                        Console.ReadLine();
                        return;
                    }
                }
                else if (((IPrincipal)userProfile).IsInRole("Admin User"))
                {
                    menu = new AdminView();
                    response = menu.DisplayMenu(ref userAccount, ref userProfile);
                    if (!response.isSuccessful && response.errorMessage == "")
                    {
                        Console.WriteLine(response.errorMessage + "\nPress Enter to Continue...");
                        Console.ReadLine();
                        return;
                    }
                    if (!response.isSuccessful && response.errorMessage != "")
                    {
                        Console.WriteLine(response.errorMessage + "\nPress Enter to Continue...");
                        Console.ReadLine();
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("huh\nPress Enter to Continue...");
                    Console.ReadLine();
                    return;
                }
            }
        }
    }
}