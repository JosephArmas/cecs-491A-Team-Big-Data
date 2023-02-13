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
        public Response DeleteAccount(UserProfile user, UserProfile del)
        {
            IDeletionService deletionService = new AccDeletionService(del);
            var answer = new Response();
            if ((user._username == del._username && user.Identity.IsAuthenticated) || ((IPrincipal)user).IsInRole("Admin User"))
            {
                Task<Response> task = deletionService.DeleteProfile();
            }
            else
            {
                var err = "User does not have permission to delete the account account";
                answer.isSuccessful = false;
                answer.errorMessage = err;
                answer.data = 0;
            }
            
            return answer;
        }
    }
}
