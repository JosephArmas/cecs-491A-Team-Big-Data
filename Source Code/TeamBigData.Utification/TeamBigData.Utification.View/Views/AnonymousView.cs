using System.Security.Cryptography;
using System.Security.Principal;
using System.Text.RegularExpressions;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Manager.Abstractions;
using TeamBigData.Utification.Models;
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
            IRegister registerer = new SecurityManager();
            ILogin login = new SecurityManager();
            bool flag = false;
            String email = "";
            String userPass = "";
            if (!((IPrincipal)userProfile).IsInRole("Anonymous User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to view";
                return response;
            }
            Console.Clear();
            Console.WriteLine("\nWelcome Anonymouse User");
            Console.WriteLine("------------MENU------------");
            Console.WriteLine("[1] Create a New Account");
            Console.WriteLine("[2] Login");
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
                    while (!flag)
                    {
                        Console.Write("\nTo create a new Account, please enter your email: ");
                        email = Console.ReadLine();
                        if (!IsValidEmail(email))
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
                        if (!IsValidPassword(userPass))
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
                    flag = false;
                    while (!flag)
                    {
                        Console.Write("\nPlease enter your Username: ");
                        email = Console.ReadLine();
                        Console.Write("\nPlese enter your Password: ");
                        userPass = Console.ReadLine();
                        if (IsValidEmail(email) && IsValidPassword(userPass))
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
                    response = login.LoginUser(email, encryptedPasswordLogin, encryptorLogin, ref userAccount, ref userProfile).Result;
                    if (!response.isSuccessful)
                    {
                        Console.WriteLine(response.errorMessage + "\nPress Enter to Continue...");
                        Console.ReadLine();
                        break;
                    }
                    flag= false;
                    Console.Clear();
                    while (!flag)
                    {
                        // Implement Count to Disable
                        Console.WriteLine("\nOTP: "+userAccount._otp);
                        Console.Write("Please enter the OTP to finish Authentication: ");
                        String enteredOTP = Console.ReadLine();
                        if (userAccount.VerifyOTP(enteredOTP).isSuccessful)
                        {
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
                            userAccount = new UserAccount();
                            userProfile = new UserProfile();
                            response.isSuccessful = true;
                            response.errorMessage = "Failed OTP.";
                            return response;
                        }
                    }
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

        public static bool IsValidPassword(String password)
        {
            Regex passwordAllowedCharacters = new Regex(@"^[a-zA-Z0-9@.,!\s-]*$");
            if (passwordAllowedCharacters.IsMatch(password) && password.Length >= 8)
                return true;
            else
                return false;
        }

        public static bool IsValidEmail(String email)
        {
            Regex emailAllowedCharacters = new Regex(@"^[a-zA-Z0-9@.-]*$");
            if (emailAllowedCharacters.IsMatch(email) && email.Contains('@') && (!email.StartsWith("@")))
                return true;
            else
                return false;
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
