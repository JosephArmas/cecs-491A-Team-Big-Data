using TeamBigData.Utification.SQLDataAccess;
using MainMenu;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;

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

        do
        {
            Menu.displayMenu();
            input = Console.ReadLine();

            switch (Int32.Parse(input))
            {
                case 0:
                    repeat = false;
                    break;
                case 1:
                    Menu.clearMenu();
                    break;
                case 2:
                    Menu.clearMenu();
                    break;
                case 3:
                    Menu.clearMenu();
                    Console.WriteLine();
                    break;
                case 4:
                    Menu.clearMenu();
                    Console.WriteLine("Hello, World!");
                    var test = new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User Id=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True");
                    var log = new Logger(test);
                    var finale = log.Log("INSERT INTO dbo.Logs (Message) VALUES ('" + DateTime.UtcNow.ToString() + " / This is an automated test Info View')");
                    if (finale.Result.isSuccessful == false && finale.Result.errorMessage != null)
                    {
                        Console.WriteLine(finale.Result.errorMessage);
                    }
                    break;
                case 5:
                    Menu.clearMenu();
                    break;
                case 6:
                    Menu.clearMenu();
                    break;
                case 7:
                    Menu.clearMenu();
                    break;
                case 8:
                    Menu.clearMenu();
                    break;
                case 9:
                    Menu.clearMenu();
                    Console.WriteLine();
                    //Yes
                    break;
            }
        } while (repeat);
    }

}