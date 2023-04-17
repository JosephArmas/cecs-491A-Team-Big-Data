﻿using System.Collections;
using System.Security.Principal;
using System.Text;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Manager.Abstractions;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.View.Abstraction;
using TeamBigData.Utification.View.Views;
using TeamBigData.Utification.EventService;

namespace TeamBigData.Utification // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private static readonly DBConnectionString connString = new DBConnectionString();
        static void Main(string[] args)
        {
            var dao = new SqlDAO(connString._connectionStringFeatures);
            var eventService = new EventService.EventService();
            var eventManager = new EventManager.EventManager();
            Console.WriteLine("title: ");
            var title = Console.ReadLine();
            Console.WriteLine("description: ");
            var description = Console.ReadLine();
            eventManager.CreateNewEvent(title, description);
            // Console.WriteLine(result);
            

            /*
            var response = new Response();
            UserProfile userProfile = new UserProfile(0);
            String userHash = "";
            IView menu = new AnonymousView();
            while (true)
            {
                if (((IPrincipal)userProfile).IsInRole("Anonymous User"))
                {
                    menu = new AnonymousView();
                    response = menu.DisplayMenu(ref userProfile, ref userHash);
                    if (!response.isSuccessful && response.errorMessage == "")
                    {
                        Console.Clear();
                        Console.WriteLine("\nExiting Utification...\nPress Enter to Continue...");
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
                    response = menu.DisplayMenu(ref userProfile, ref userHash);
                    if (!response.isSuccessful && response.errorMessage == "")
                    {
                        Console.WriteLine("\nExiting Utification...\nPress Enter to Continue...");
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
                    response = menu.DisplayMenu(ref userProfile, ref userHash);
                    if (!response.isSuccessful && response.errorMessage == "")
                    {
                        Console.WriteLine("\nExiting Utification...\nPress Enter to Continue...");
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
            */
        }
    }
}