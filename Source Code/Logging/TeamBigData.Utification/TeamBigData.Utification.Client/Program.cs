using System.ComponentModel.DataAnnotations;
using TeamBigData.Utification.Cross;
using TeamBigData.Utification.LogBusiness;

namespace TeamBigData.Utification.Client
{
    class Program
    {
        private static BusRules businessLayer = new BusRules();
        public Program(int id, int loglevel, int category, string desc)
        {

        }
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
                        Console.WriteLine(CreatePin());
                        break;
                    case 4:
                        Menu.clearMenu();
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
                        Console.WriteLine(logDel());
                        break;
                }
            } while (repeat);
        }
        public static Results userLog(string opr,string desc)
        {
            return businessLayer.BusLog(DateTime.UtcNow.ToString(),Loglevel.Info,opr,Category.View,desc);
        }
        public static string CreatePin()
        { //Need to create a new table of one that includes all of the data types
            Results beenLogged = userLog("Create Pin","User created a pin");
            if (beenLogged == Results.Pass)
            {
                return "Pin has been created";
            }
            else
            {
                return "Pin has not been created";
            }
        }
        public static Results userDel()
        {
            return businessLayer.BusDel();
        }
        public static string logDel()
        {
            Results Del = userDel();
            if(Del == Results.Pass)
            {
                return "Logs have been cleared";
            }
            else
            {
                return "Logs have not been cleared";
            }
        }
    }
}