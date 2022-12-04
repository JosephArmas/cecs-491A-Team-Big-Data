using TeamBigData.Utification.SQLDataAccess;
using MainMenu;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.ManagerLayer;
using TeamBigData.Utification.Security;
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
                        if(userManager.IsAuthenticated())
                        {
                            Console.WriteLine("To create a new Account, please enter your email");
                            String email = Console.ReadLine();
                            Console.WriteLine("Please enter your new password");
                            String userPassword = Console.ReadLine();
                            var response = userManager.InsertUser(email, userPassword);
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
                            Console.WriteLine(result.errorMessage);
                            if(result.isSuccessful)
                            {
                                Console.WriteLine("Please Enter This Password to Finish Authentication");
                                Console.WriteLine(userManager.SendOTP());
                                var otp = Console.ReadLine();
                                var result2 = userManager.ReceiveOTP(otp);
                                Console.WriteLine(result2.errorMessage);
                            }
                        }
                        else
                        {
                            Console.WriteLine("You are already Logged In, Please Log Out Before Logging In again");
                        }
                        break;
                    case 3:
                        Menu.clearMenu();
                        var pinLogger = new Logger(new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
                        var logResult = pinLogger.Log("INSERT INTO dbo.Logs (CorrelationID,LogLevel,[User],[DateTime],[Event],Category,[Message]) VALUES (4, 'Info','SYSTEM','" + 
                            DateTime.UtcNow.ToString() + "', 'CreatePin', 'View','This is a automated test for finding if it took longer than 5 seconds')");
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