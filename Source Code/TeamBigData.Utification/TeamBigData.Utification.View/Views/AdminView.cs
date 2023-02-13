using System.Security.Principal;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.View.Abstraction;

namespace TeamBigData.Utification.View.Views
{
    public class AdminView : IView
    {
        /// <summary>
        /// Display all startup options.
        /// </summary>
        public Response DisplayMenu(ref UserAccount userAccount, ref UserProfile userProfile)
        {
            Response response = new Response();
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to view";
                return response;
            }
            Console.WriteLine("Welcome Admin User");
            Console.WriteLine("---------MENU---------");
            Console.WriteLine("[1] View All User Accounts");
            Console.WriteLine("[2] View All User Profiles");
            Console.WriteLine("[3] View Account Recovery Requests");
            Console.WriteLine("[4] Re-enable User");
            Console.WriteLine("[5] LogOut");
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
                    Console.Clear();
                    SecurityManager secManagerAcc = new SecurityManager();
                    List<UserAccount> listAcc = new List<UserAccount>();
                    response = secManagerAcc.GetUserAccountTable(listAcc, userProfile);
                    if (!response.isSuccessful)
                    {
                        Console.Clear();
                        Console.WriteLine(response.errorMessage);
                        Console.WriteLine("Press Enter to exit...");
                        Console.ReadLine();
                        response.isSuccessful = false;
                        return response;
                    }
                    Console.WriteLine("Printing out User Account Table");
                    for (int i = 0; i < listAcc.Count; i++)
                        Console.WriteLine(((UserAccount)listAcc[i]).ToString());
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
                case 2:
                    Console.Clear();
                    SecurityManager secManager = new SecurityManager();
                    List<UserProfile> list = new List<UserProfile>();
                    response = secManager.GetUserProfileTable(list, userProfile);
                    if (!response.isSuccessful)
                    {
                        Console.Clear();
                        Console.WriteLine(response.errorMessage);
                        Console.WriteLine("Press Enter to exit...");
                        Console.ReadLine();
                        response.isSuccessful = false;
                        return response;
                    }
                    Console.WriteLine("Printing out User Profile Table");
                    for (int i = 0; i < list.Count; i++)
                        Console.WriteLine(((UserProfile)list[i]).ToString());
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
                case 3:
                    Console.Clear();
                    SecurityManager secManagerAccRecovery = new SecurityManager();
                    List<string> listRequests = new List<string>();
                    response = secManagerAccRecovery.GetRecoveryRequests(listRequests, userProfile);
                    if (!response.isSuccessful)
                    {
                        Console.Clear();
                        Console.WriteLine(response.errorMessage);
                        Console.WriteLine("Press Enter to exit...");
                        Console.ReadLine();
                        response.isSuccessful = false;
                        return response;
                    }
                    Console.Clear();
                    Console.WriteLine("Printing out Recovery Requests");
                    for (int i = 0; i < listRequests.Count; i++)
                        Console.WriteLine(listRequests[i]);
                    Console.WriteLine("press Enter to exit...");
                    Console.ReadLine();
                    break;
                case 4:
                    Console.Clear();
                    SecurityManager secManagerEnable = new SecurityManager();
                    Console.WriteLine("Please Enter the name of the User to be re-enabled");
                    String disUser = Console.ReadLine();
                    response = secManagerEnable.EnableAccount(disUser,userProfile);
                    if (!response.isSuccessful)
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
                case 5:
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
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    return response;
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
