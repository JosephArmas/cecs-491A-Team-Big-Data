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
        public Task<Response> SelectUserProfile(ref UserProfile userProfile, int userID);
        public Task<Response> SelectUserProfileTable(ref List<UserProfile> profileList, String role);
        public Task<Response> SelectUserAccount(ref UserAccount userAccount, String username);
        public Task<Response> SelectUserAccountTable(ref List<UserAccount> listAccounts, String role);
        public Task<Response> SelectLastUserID();
        public Task<List<Pin>> SelectPinTable();
        public Task<Response> SelectNewReputation(Report report);
    }
}
