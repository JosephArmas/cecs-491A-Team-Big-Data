using System.Security.Cryptography;
using System.Security.Principal;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Manager;
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
            SecurityManager securityManager = new SecurityManager();
            if (!((IPrincipal)userProfile).IsInRole("Anonymous User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to view";
                return response;
            }
            Console.WriteLine("Welcome Anonymouse User");
            Console.WriteLine("------------MENU------------");
            Console.WriteLine("[1] Create a New Account");
            Console.WriteLine("[2] Login");
            Console.WriteLine("[3] Account Recovery/Forgot Password");
            Console.WriteLine("[0] exit");
            Console.WriteLine("Enter 0-2");
            string input = Console.ReadLine();
            switch (Int32.Parse(input))
            {
                case 0:
                    //Console.Clear();
                    Console.WriteLine("Exiting Utification...");
                    response.isSuccessful = false;
                    response.errorMessage = "";
                    return response;
                case 1:
                    Console.WriteLine("To create a new Account, please enter your email");
                    String email = Console.ReadLine();
                    Console.WriteLine("Please enter your new password");
                    String userPassword = Console.ReadLine();
                    var encryptor = new Encryptor();
                    var encryptedPassword = encryptor.encryptString(userPassword);
                    response = securityManager.InsertUser(email, encryptedPassword, encryptor);
                    Console.WriteLine(response.errorMessage);
                    break;

                case 2:
                    Console.WriteLine("Please enter your username");
                    String username = Console.ReadLine();
                    Console.WriteLine("Plese enter your password");
                    String password = Console.ReadLine();
                    var encryptor2 = new Encryptor();
                    var encryptedPassword2 = encryptor2.encryptString(password);
                    response = securityManager.VerifyUser(username, encryptedPassword2, encryptor2).Result;
                    if(response.isSuccessful)
                    {
                        Console.WriteLine("Please enter the OTP to finish Authentication");
                        Console.WriteLine(securityManager.SendOTP());
                        string enteredOtp = Console.ReadLine();
                        response = securityManager.LoginOTP(enteredOtp);
                        if(response.isSuccessful)
                        {
                            Console.WriteLine("You have been Successfully Authenticated");
                            userProfile = response.data as UserProfile;
                        }
                    }
                    else
                    {
                        Console.WriteLine(response.errorMessage);
                    }
                    break;

                case 3:
                    Console.WriteLine("Please enter your username");
                    String inputUsername = Console.ReadLine();
                    Console.WriteLine("Please enter your new password");
                    String newPassword = Console.ReadLine();
                    securityManager.GenerateOTP();
                    Console.WriteLine("Please Enter the OTP: " + securityManager.SendOTP());
                    String otp = Console.ReadLine();
                    Console.WriteLine(securityManager.RecoverAccount(inputUsername, newPassword, otp).Result.errorMessage);
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
