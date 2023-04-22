using System;
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
using System.Diagnostics;

namespace TeamBigData.Utification.View.Views
{
    public class UserManagementView: IView
    {
        /// <summary>
        /// Display options for User Management. Admin Use only.
        /// </summary>
        public  Response DisplayMenu(ref UserProfile userProfile, ref String userhash)
        {
            Response response = new Response();
            SecurityManager securityManager = new SecurityManager();
            var stopwatch = new Stopwatch();
            var expected = 5000;
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
            Console.WriteLine("[4] Disable User");
            Console.WriteLine("[5] Enable User");
            Console.WriteLine("[6] Bulk File Upload");
            Console.WriteLine("[7] TEST create testSize.csv");
            Console.WriteLine("[8] LogOut");
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
                    byte[] encryptedPassword = encryptor.encryptString(userPassword);
                    Console.WriteLine("Do you want:\n[1] This User to be Regular\n[2] This User to be an Admin");
                    String adminCreate = Console.ReadLine();
                    var check = true;
                    while (check != false)
                    {
                        if (adminCreate == "1")
                        {
                            stopwatch.Start();
                            response = securityManager.RegisterUser(email, encryptedPassword, encryptor).Result;
                            stopwatch.Stop();
                            var actualReg = stopwatch.ElapsedMilliseconds;
                            if(response.isSuccessful & (actualReg < expected))
                            {
                                Console.WriteLine("UM operation was successful");
                            }
                            else
                            {
                                response.isSuccessful = false;
                                response.errorMessage = "UM operation was unsuccessful";
                            }
                            check = false;
                        }
                        else if (adminCreate == "2")
                        {
                            stopwatch.Start();
                            response = securityManager.RegisterUserAdmin(email, encryptedPassword, encryptor, userProfile).Result;
                            stopwatch.Stop();
                            var actualAdmin = stopwatch.ElapsedMilliseconds;
                            if (response.isSuccessful & (actualAdmin< expected))
                            {
                                Console.WriteLine("UM operation was successful");
                            }
                            else
                            {
                                response.isSuccessful = false;
                                response.errorMessage= "UM operation was unsuccessful";
                            }
                            check = false;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Entry. Try again");
                            Console.WriteLine("Do you want:\n[1] This User to be Regular\n[2] This User to be an Admin");
                            adminCreate = Console.ReadLine();

                        }
                    }
                    return response;
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
                    var pii = 1;

                    stopwatch.Start();
                    response = secManagerDelete.DeleteProfile(delUser, userProfile).Result;
                    stopwatch.Stop();
                    var actualDel = stopwatch.ElapsedMilliseconds;
                    if (response.isSuccessful & (actualDel < expected))
                    {
                        Console.WriteLine("UM operation was successful");
                    }
                    else if (!response.isSuccessful)
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
                    stopwatch.Start();
                    var userDao = new SqlDAO(connectionString);
                    var encryptorU = new Encryptor();
                    var encryptedPasswordU = encryptorU.encryptString(updatePass);


                    //var digest = SecureHasher.HashString(updatedUser, updatePass);
                    //response = userDao.ChangePassword(line.email, digest).Result;
                    response = secManagerUpdate.ChangePassword(updatedUser, userProfile, encryptorU, encryptedPasswordU).Result;
                    stopwatch.Stop();
                    var actualUpdate = stopwatch.ElapsedMilliseconds;
                    if (response.isSuccessful & (actualUpdate < expected))
                    {
                        Console.WriteLine("UM operation was successful");
                    }
                    else if (!response.isSuccessful)
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
                    stopwatch.Start();
                    response = secManagerDisable.DisableAccount(disabledUser, userProfile).Result;
                    stopwatch.Stop();
                    var actualDisable = stopwatch.ElapsedMilliseconds;
                    if (response.isSuccessful & (actualDisable < expected))
                    {
                        Console.WriteLine("UM operation was successful");
                    }
                    else if (!response.isSuccessful)
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
                    stopwatch.Start();
                    response = secManagerEnable.EnableAccount(disUser, userProfile).Result;
                    stopwatch.Stop();
                    var actualEnable = stopwatch.ElapsedMilliseconds;
                    if (response.isSuccessful & (actualEnable < expected))
                    {
                        Console.WriteLine("UM operation was successful");
                    }
                    else if (!response.isSuccessful)
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
                    Console.WriteLine("Specify file name like 'file.csv'");
                    //https://learn.microsoft.com/en-us/dotnet/api/system.io.path.getfilename?view=net-7.0#system-io-path-getfilename(system-string)
                    //Universal Naming Convention is C:\MyDir\MyFile.csv
                    var filenameGet = Console.ReadLine();
                    var filename = @"C:\MyDir\"+filenameGet+"";
                    //var filename = "C:\Users\joshu\source\repos\cecs-491A-Team-Big-Data\Source Code\TeamBigData.Utification\TeamBigData.Utification.View\bin\Debug\net6.0\""+ filename+"";

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
                    Console.WriteLine("Bulk UM operation was Successful");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
                case 7:
                    Console.Clear();
                    /*ran into errors with getting directory path*/
                    string directoryPath = Path.Combine(Environment.CurrentDirectory, @"C:\MyDir");
                    /*Directory will be created if not existing*/
                    Directory.CreateDirectory(directoryPath);
                    //DirectoryInfo di = new DirectoryInfo(@"c:\MyDir");
                    DirectoryInfo di = new DirectoryInfo(directoryPath);
                    string filePath = Path.Combine(directoryPath, "testSize.csv");
                    Console.WriteLine("Creating File...");
                    try
                    {
                        //using (StreamWriter sw = new StreamWriter("testSize.csv"))
                        using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
                        {
                            //excel has  a limit of 1million rows. it is equal to .04 of a GB
                            //so it needs to be 50 times that to reach 2GB (50 was barely not enough)
                            //doing this will take like 4~5min
                            for (int line = 0; line < 51500000; line++)
                            {
                                //the $ allows me to insert info into the string
                                string data = $"CREATE,testSize{line}@yahoo.com,password";
                                sw.WriteLine(data);
                                if (line == 25999999)
                                {
                                    Console.WriteLine("Halfway done...");
                                    break;
                                }
                                sw.Flush();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    Console.WriteLine("testSize.csv was created");
                    Console.ReadLine();
                    break;
                case 8:
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

