using TeamBigData.Utification.SQLDataAccess;
using MainMenu;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.ManagerLayer;

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
                        Console.WriteLine("To create a new Account, please enter your email");
                        String email = Console.ReadLine();
                        Console.WriteLine("Please enter your new password");
                        String password = Console.ReadLine();
                        var manager = new Manager();
                        var result = manager.InsertUser(email, password);
                        Console.WriteLine(result.errorMessage);
                        break;
                    case 2:
                        Menu.clearMenu();
                        break;
                    case 3:
                        Menu.clearMenu();
                        break;
                }
            }
        } while (repeat);
    }

}