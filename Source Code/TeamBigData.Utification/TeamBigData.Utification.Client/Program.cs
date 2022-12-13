using TeamBigData.Utification.SQLDataAccess;
using MainMenu;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.ManagerLayer;
using TeamBigData.Utification.Security;
using TeamBigData.Utification.Models;
using System.Text;
using System.ComponentModel;
//Adapted from CECS 475 Phuong Nguyen

class Program
{
    static void Main(string[] args)
    {
        run();
    }
    public static void run()
    {
        bool repeat = true;
        string input;
        var userManager = new Manager();

        do
        {
            Menu.displayMenu();
            input = Console.ReadLine();
            if (input == null)
            {
                Menu.clearMenu();
                Console.WriteLine("Invalid input");
            }
            else
            {
                switch (Int32.Parse(input))
                {
                    case 0:
                        repeat = false;
                        break;
                    case 1:
                        Menu.clearMenu();
                        if(!userManager.IsAuthenticated())
                        {
                            Console.WriteLine("To create a new Account, please enter your email");
                            String email = Console.ReadLine();
                            Console.WriteLine("Please enter your new password");
                            String userPassword = Console.ReadLine();
                            var encryptor = new Encryptor();
                            var encryptedPassword = encryptor.encryptString(userPassword);
                            var response = userManager.InsertUser(email, encryptedPassword, encryptor);
                            Console.WriteLine(response.errorMessage);
                        }
                        else
                        {
                            Console.WriteLine("You are already Logged In, Please Log Out Before Creating an Account");
                        }
                        break;
                    case 2:
                        Menu.clearMenu();
                        if(!userManager.IsAuthenticated())
                        {

                            Console.WriteLine("Please enter your Username");
                            String loginUsername = Console.ReadLine();
                            Console.WriteLine("Please enter your Password");
                            String password = Console.ReadLine();
                            var encryptor = new Encryptor();
                            var encryptedPassword = encryptor.encryptString(password);
                            var result = userManager.VerifyUser(loginUsername, encryptedPassword, encryptor);
                            if(result.isSuccessful)
                            {
                                Console.WriteLine("Please Enter the OTP to finish Authentication");
                                Console.Write(userManager.SendOTP());
                                String otp = Console.ReadLine();
                                var otpResult = userManager.VerifyOTP(otp);
                                Console.WriteLine(otpResult.errorMessage);
                                Boolean tryAgain = true;
                                string response;
                                while (tryAgain && !otpResult.isSuccessful)
                                {
                                    Console.WriteLine("Please Try Again");
                                    otp = Console.ReadLine();
                                    otpResult = userManager.VerifyOTP(otp);
                                    Console.WriteLine(otpResult.errorMessage);
                                    if (!otpResult.isSuccessful)
                                    {
                                        Console.WriteLine("Would you like to try again (y/n)");
                                        response = Console.ReadLine();
                                        if (response.Equals("y"))
                                        {
                                            tryAgain = true;
                                        }
                                        else
                                        {
                                            tryAgain = false;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine(result.errorMessage);
                            }
                        }
                        else
                        {
                            Console.WriteLine("You are already Logged In, Please Log Out Before Logging In again");
                        }
                        break;
                    case 3:
                        Menu.clearMenu();
                        var result3 = userManager.LogOut();
                        Console.WriteLine(result3.errorMessage);
                        break;
                    case 4:
                        Menu.clearMenu();
                        var pinLogger = new Logger(new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
                        var log = new Log(4, "Info", "SYSTEM", "CreatePin", "View", "This is a automated test for finding if it took longer than 5 seconds");
                        var logResult = pinLogger.Log(log);
                        if (logResult.Result.isSuccessful)
                        {
                            Console.WriteLine("Pin has been created");
                        }
                        else
                        {
                            Console.WriteLine("Pin was not created");
                        }
                        break;
                }
            }
        } while (repeat);
    }

}