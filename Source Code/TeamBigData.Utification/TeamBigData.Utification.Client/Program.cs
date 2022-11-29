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
        string username = null;

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
                        if(username == null)
                        {
                            Console.WriteLine("To create a new Account, please enter your email");
                            String email = Console.ReadLine();
                            Console.WriteLine("Please enter your new password");
                            String userPassword = Console.ReadLine();
                            var userManager = new Manager();
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
                        if(username == null)
                        {

                            Console.WriteLine("Please enter your Username");
                            String loginUsername = Console.ReadLine();
                            Console.WriteLine("Please enter your Password");
                            String password = Console.ReadLine();
                            var encryptor = new Encryptor();
                            var encryptedPassword = encryptor.encryptString(password);
                            var manager = new Manager();
                            var result = manager.AuthenticateUser(loginUsername, encryptedPassword, encryptor);
                            Console.WriteLine(result.errorMessage);
                            if (result.isSuccessful)
                            {
                                username = loginUsername;
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