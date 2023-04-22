using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions
{
    public interface IUsersDBSelecter
    {
        public Task<DataResponse<UserAccount>> SelectUserAccount(String username);
        public Task<DataResponse<UserProfile>> SelectUserProfile(int userID);
    }
}
