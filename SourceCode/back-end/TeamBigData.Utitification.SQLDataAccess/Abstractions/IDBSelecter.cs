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
        public Task<DataResponse<UserProfile>> SelectUserProfile(int userID);
        public Task<DataResponse<List<UserProfile>>> SelectUserProfileTable(String role);
        public Task<DataResponse<UserAccount>> SelectUserAccount(String username);
        public Task<DataResponse<List<UserAccount>>> SelectUserAccountTable(String role);
        public Task<Response> SelectLastUserID();
    }
}
