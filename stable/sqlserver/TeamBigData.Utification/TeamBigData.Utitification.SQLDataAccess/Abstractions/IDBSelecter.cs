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
        public Task<UserProfile> SelectUserProfile(String username);
        public Task<Response> SelectUserProfileTable(ref List<UserProfile> profileList);
        public Task<UserAccount> SelectUserAccount(String username);
        public Task<Response> SelectUserAccountTable(ref List<UserAccount> accountList);
    }
}
