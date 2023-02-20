﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.View.Abstraction;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.AccountServices;

namespace TeamBigData.Utification.View.Views
{
    public class UserManagementView: IView
    {
        /// <summary>
        /// Display options for User Management. Admin Use only.
        /// </summary>
        public  Response DisplayMenu(ref UserAccount userAccount, ref UserProfile userProfile)
        {
            Response response = new Response();
            SecurityManager securityManager = new SecurityManager();
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to view";
                return response;
            }
            Console.WriteLine("Welcome Admin User");
            Console.WriteLine("---------MENU---------");
            Console.WriteLine("[1] Create Account");
            Console.WriteLine("[2] Delete Account");
            Console.WriteLine("[3] Update Account");
            Console.WriteLine("[4] Enable User");
            Console.WriteLine("[5] Disable User");
            Console.WriteLine("[6] Bulk File Upload");
            Console.WriteLine("[7] LogOut");
            Console.WriteLine("[0] exit");
            Console.WriteLine("Enter 0-4");
            string input = Console.ReadLine();
            switch (Int32.Parse(input))
            {
                case 0:
                    //Console.Clear();
                    Console.WriteLine("Logging Out User...\nExiting Utification...");
                    response.isSuccessful = false;
                    response.errorMessage = "";
                    return response;
                case 1:
                    //creating an account
                    Console.WriteLine("To create a new Account, please enter an email");
                    String email = Console.ReadLine();
                    Console.WriteLine("Please enter new password");
                    String userPassword = Console.ReadLine();
                    var encryptor = new Encryptor();
                    var encryptedPassword = encryptor.encryptString(userPassword);
                    Console.WriteLine("Do you want:\n[1] This User to be Regular\n[2] This User to be an Admin");
                    String adminCreate = Console.ReadLine();
                    var check = true;
                    while (check != false)
                    {
                        if (adminCreate == "1")
                        {
                            response = securityManager.InsertUser(email, encryptedPassword, encryptor);
                            check = false;
                        }
                        else if (adminCreate == "2")
                        {
                            response = securityManager.InsertUserAdmin(email, encryptedPassword, encryptor, userProfile);
                            check = false;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Entry. Try again");
                            Console.WriteLine("Do you want:\n[1] This User to be Regular\n[2] This User to be an Admin");
                            adminCreate = Console.ReadLine();

                        }
                    }
                   
                    break;
                case 2:
                    //Deleting account
                    Console.Clear();
                    SecurityManager secManagerDelete = new SecurityManager();
                    //var userDao = new SqlDAO(connectionString);
                    //response = await userDao.GetUser(userAccount);
                    //response = await userDao.UpdateUserProfile(userProfile);
                    Console.WriteLine("Please Enter the name of the User to be Deleted");
                    String delUser = Console.ReadLine();
                    response = secManagerDelete.DeleteProfile(delUser, userProfile);
                    if (!response.isSuccessful)
                    {
                        Console.Clear();
                        Console.WriteLine(response.errorMessage);
                        Console.WriteLine("Press Enter to exit...");
                        Console.ReadLine();
                        response.isSuccessful = false;
                        return response;
                    }
                    Console.WriteLine("User account was successfully updated");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();

                    break;
                case 3:
                    //Updating account
                    Console.Clear();
                    SecurityManager secManagerUpdate = new SecurityManager();
                    //var userDao = new SqlDAO(connectionString);
                    //response = await userDao.GetUser(userAccount);
                    //response = await userDao.UpdateUserProfile(userProfile);
                    Console.WriteLine("Please Enter the name of the User to be updated");
                    String updatedUser = Console.ReadLine();
                    Console.WriteLine("What new password would you like?");
                    String updatePass = Console.ReadLine();
                    var userDao = new SqlDAO(connectionString);
                    var encryptorU = new Encryptor();
                    var encryptedPasswordU = encryptorU.encryptString(updatePass);


                    //var digest = SecureHasher.HashString(updatedUser, updatePass);
                    //response = userDao.ChangePassword(line.email, digest).Result;
                    response = secManagerUpdate.ChangePassword(updatedUser, userProfile, encryptorU, encryptedPasswordU);
                    if (!response.isSuccessful)
                    {
                        Console.Clear();
                        Console.WriteLine(response.errorMessage);
                        Console.WriteLine("Press Enter to exit...");
                        Console.ReadLine();
                        response.isSuccessful = false;
                        return response;
                    }
                    Console.WriteLine("User account was successfully updated");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();

                    break;
                case 4:
                    //disabling a user
                    Console.Clear();
                    SecurityManager secManagerDisable = new SecurityManager();
                  
                    Console.WriteLine("Please Enter the name of the User to be disabled");
                    String disabledUser = Console.ReadLine();
                    response = secManagerDisable.DisableAccount(disabledUser, userProfile);
                    if (!response.isSuccessful)
                    {
                        Console.Clear();
                        Console.WriteLine(response.errorMessage);
                        Console.WriteLine("Press Enter to exit...");
                        Console.ReadLine();
                        response.isSuccessful = false;
                        return response;
                    }
                    Console.WriteLine("User account was successfully disabled");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
                case 5:
                    //enabling a user
                    Console.Clear();
                    SecurityManager secManagerEnable = new SecurityManager();
                    Console.WriteLine("Please Enter the name of the User to be re-enabled");
                    String disUser = Console.ReadLine();
                    response = secManagerEnable.EnableAccount(disUser, userProfile);
                    if (!response.isSuccessful)
                    {
                        Console.Clear();
                        Console.WriteLine(response.errorMessage);
                        Console.WriteLine("Press Enter to exit...");
                        Console.ReadLine();
                        response.isSuccessful = false;
                        return response;
                    }
                    Console.WriteLine("User account was successfully re-enabled");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
                case 6:
                    //Bulk File Upload
                    Console.Clear();
                    //SecurityManager secManager = new SecurityManager();
                    Console.WriteLine("Please upload CSV for requests");
                    var filename = Console.ReadLine();
                    //var csvReader = new List<CsvReader>();
                    CsvReader csvReader = new CsvReader();

                    response = csvReader.BulkFileUpload(filename, userProfile).Result;
                    if (!response.isSuccessful)
                    {
                        Console.Clear();
                        Console.WriteLine(response.errorMessage);
                        Console.WriteLine("Press Enter to exit...");
                        Console.ReadLine();
                        response.isSuccessful = false;
                        return response;
                    }
                    Console.WriteLine("User account was successfully re-enabled");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
                case 7:
                    Console.Clear();
                    SecurityManager secManagerLogout = new SecurityManager();
                    response = secManagerLogout.LogOut();
                    if (!response.isSuccessful)
                    {
                        Console.Clear();
                        Console.WriteLine(response.errorMessage);
                        Console.WriteLine("Press Enter to exit...");
                        Console.ReadLine();
                        response.isSuccessful = false;
                        return response;
                    }
                    Console.WriteLine("Successfully logged out");
                    
                    return response;
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

