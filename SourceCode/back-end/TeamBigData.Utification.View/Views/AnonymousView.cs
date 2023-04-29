using System.Security.Cryptography;
using System.Security.Principal;
using System.Globalization;
using System.Text.RegularExpressions;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Manager.Abstractions;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.View.Abstraction;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.AnalysisManagers;

namespace TeamBigData.Utification.View.Views
{
    public class AnonymousView : IView
    {
        public CultureInfo culStandard = new CultureInfo("en-US");
        public CultureInfo culCurrent = CultureInfo.CurrentCulture;
        /// <summary>
        /// Display all startup options.
        /// </summary>
        public Response DisplayMenu(ref UserProfile userProfile, ref String userHash)
        {
            Response response = new Response();
            IRegister registerer = new SecurityManager();
            ILogin login = new SecurityManager();
            bool flag = false;
            String email = "";
            String userPass = "";
            InputValidation valid = new InputValidation();
            if (!((IPrincipal)userProfile).IsInRole("Anonymous User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to view";
                return response;
            }
            Console.WriteLine("\nWelcome Anonymouse User");
            Console.WriteLine("------------MENU------------");
            Console.WriteLine("[1] Create a New Account");
            Console.WriteLine("[2] Login");
            Console.WriteLine("[3] Recover Account/Forgot Password");
            Console.WriteLine("[0] exit");
            Console.Write("Enter 0-2: ");
            string input = Console.ReadLine();
            switch (input)
            {
                case "0":
                    response.isSuccessful = false;
                    response.errorMessage = "";
                    return response;
                case "1":
                    Console.Clear();
                    valid = new InputValidation();
                    while (!flag)
                    {
                        Console.Write("\nTo create a new Account, please enter your email: ");
                        email = Console.ReadLine();
                        if (!valid.IsValidEmail(email))
                        {
                            Console.Clear();
                            Console.WriteLine("\nInvalid Email...");
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                    flag = false;
                    while (!flag)
                    {
                        Console.Write("\nPlease enter your new password: ");
                        userPass = Console.ReadLine();
                        if (!valid.IsValidPassword(userPass))
                        {
                            Console.WriteLine("\nInvalid Password...");
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                    var encryptorRegister = new Encryptor();
                    var encryptedPasswordRegister = encryptorRegister.encryptString(userPass);
                    response = registerer.RegisterUser(email, encryptedPasswordRegister, encryptorRegister).Result;
                    if (!response.isSuccessful)
                    {
                        Console.WriteLine(response.errorMessage + "\nPress Enter to Try Again...");
                        break;
                    }
                    Console.WriteLine(response.errorMessage + "\nPress Enter to Login...");
                    Console.ReadLine();
                    flag= false;
                    break;

                case "2":
                    String tryAgain = "";
                    Console.Clear();
                    valid = new InputValidation();
                    flag = false;
                    while (!flag)
                    {
                        Console.Write("\nPlease enter your Username: ");
                        email = Console.ReadLine();
                        Console.Write("\nPlese enter your Password: ");
                        userPass = Console.ReadLine();
                        if (valid.IsValidEmail(email) && valid.IsValidPassword(userPass))
                        {
                            flag = true;
                        }
                        else
                        {
                            while (tryAgain != "1")
                            {
                                // Implement Count to Disable
                                Console.Write("\nUsername or Password is Invalid.\nPress Enter 1 to Try Again or 0 to Quit Login: ");
                                tryAgain = Console.ReadLine();
                                if (tryAgain == "0")
                                {
                                    response.isSuccessful = true;
                                    response.errorMessage = "Failed to Login";
                                    return response;
                                }
                            }
                        }
                    }
                    var encryptorLogin = new Encryptor();
                    var encryptedPasswordLogin = encryptorLogin.encryptString(userPass);
                    var dataResponse = login.LoginUser(email, encryptedPasswordLogin, encryptorLogin, userProfile).Result;
                    if (!dataResponse.isSuccessful)
                    {
                        Console.WriteLine(response.errorMessage + "\nPress Enter to Continue...");
                        Console.ReadLine();
                        break;
                    }
                    flag= false;
                    Console.Clear();
                    var secManager = new SecurityManager();
                    while (!flag)
                    {
                        // Implement Count to Disable
                        secManager.GenerateOTP();
                        Console.WriteLine("\nOTP: "+ secManager.SendOTP());
                        Console.Write("Please enter the OTP to finish Authentication: ");
                        String enteredOTP = Console.ReadLine();
                        if (secManager.VerifyOTP(enteredOTP).isSuccessful)
                        {
                            userProfile = dataResponse.data;
                            userHash = dataResponse.errorMessage;
                            Console.WriteLine("\nYou have been Successfully Authenticated.\nPress Enter to Continue...");
                            Console.ReadLine();
                            response.isSuccessful = true;
                            response.errorMessage = "Successfully Authenticated.";
                            return response;
                        }
                        Console.Clear();
                        Console.Write("\nFailed OTP.\nEnter 0 to quit or Press Enter to Try Again: ");
                        string retry = Console.ReadLine();
                        if (retry == "0")
                        {
                            userProfile = new UserProfile();
                            response.isSuccessful = true;
                            response.errorMessage = "Failed OTP.";
                            return response;
                        }
                    }
                    break;
                case "3":
                    Console.WriteLine("Please enter your username");
                    String inputUsername = Console.ReadLine();
                    Console.WriteLine("Please enter your new password");
                    String newPassword = Console.ReadLine();
                    var passwordEncryptor = new Encryptor();
                    byte[] newEncryptedPassword = passwordEncryptor.encryptString(newPassword);
                    SecurityManager securityManager = new SecurityManager();
                    securityManager.GenerateOTP();
                    Console.WriteLine("Please Enter the OTP: " + securityManager.SendOTP());
                    String otp = Console.ReadLine();
                    Console.WriteLine(securityManager.RecoverAccount(inputUsername, newEncryptedPassword, passwordEncryptor).Result.errorMessage);
                    break;
                default:
                    Console.WriteLine("Invalid Input\nPress Enter to Try Again...");
                    Console.ReadLine();
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
        public CultureInfo GetCultureInfo()
        {
            return CultureInfo.CurrentCulture;
        }
        public void SetCultureInfo(CultureInfo cul)
        {
            CultureInfo.CurrentCulture = cul;
        }
    }
}
