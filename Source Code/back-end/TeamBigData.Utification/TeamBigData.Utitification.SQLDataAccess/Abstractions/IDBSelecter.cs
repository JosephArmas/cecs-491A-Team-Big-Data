using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.Abstractions
{
    public interface IDBSelecter
    {
        public Task<Response> SelectUserProfile(int userID, ref UserProfile userProfile);
        public Task<Response> SelectUserProfileTable(ref List<UserProfile> userProfiles);
        public Task<Response> SelectUserAccount(String username, ref UserAccount userAccount);
        public Task<Response> SelectUserAccountTable(ref List<UserAccount> userAccounts, string role);
        public Task<Response> SelectLastUserID();
        
    }
}
