using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.DeletionService;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.Manager
{
    public class DeletionManager
    {
        
        /// <summary>
        /// Takes in 2 user accounts to check if valid and delete the second
        /// </summary>
        /// <param name="user">The account that will activate the deletion</param>
        /// <param name="del">The account that will be deleted</param>
        /// <returns>Response if the account was deleted</returns>
        public Response DeleteAccount(String del, UserProfile user)
        {
            bool isAdmin()
            {
                return ((IPrincipal)user).IsInRole("Admin User");
            }
            IDeletionService deletionService = new AccDeletionService(del);
            var answer = new Response();
            
            if ((user.Identity.Name == del && user.Identity.IsAuthenticated) || isAdmin())
            {
                Console.WriteLine(user.Identity.Name + " " + del);
                Task<Response> taskF = deletionService.DeletePIIFeatures();
                Task<Response> taskP = deletionService.DeletePIIProfile();
                if (taskF.Result.isSuccessful == false)
                {
                    answer = taskF.Result;
                }
                else
                {
                    answer = taskP.Result;
                }
            }
            else
            {
                var err = "User does not have permission to delete the account";
                answer.isSuccessful = false;
                answer.errorMessage = err;
                answer.data = 0;
            }
            
            return answer;
        }

    }
}
