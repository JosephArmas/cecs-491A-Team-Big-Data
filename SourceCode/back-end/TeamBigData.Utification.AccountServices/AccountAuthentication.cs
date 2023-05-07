using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.LogsDB.Abstractions;
using TeamBigData.Utification.SQLDataAccess.UsersDB;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;

namespace TeamBigData.Utification.AccountServices
{
    public class AccountAuthentication
    {
        private readonly IUsersDBSelecter _usersDBSelecter;

        public AccountAuthentication(UsersSqlDAO usersSqlDAO)
        {
            _usersDBSelecter = usersSqlDAO;
        }

        public async Task<DataResponse<UserAccount>> AuthenticateUserAccount(String email, String password)
        {
            var userAccount = await _usersDBSelecter.SelectUserAccount(email).ConfigureAwait(false);

            if (!userAccount.IsSuccessful)
            {
                userAccount.IsSuccessful = false;
                userAccount.ErrorMessage += ", {failed: _usersDBSelecter.SelectUserAccount}";
                return userAccount;
            }

            var digest = SecureHasher.HashString(userAccount.Data.Salt, password);

            if (digest != userAccount.Data.Password) 
            {
                userAccount.IsSuccessful = false;
                userAccount.ErrorMessage += ", {failed: digest validation}";
                return userAccount;
            }
            else
            {
                userAccount.IsSuccessful = true;
            }

            return userAccount;
        }

        public async Task<DataResponse<UserProfile>> AuthenticatedUserProfile(int userID)
        {
            var userProfile = await _usersDBSelecter.SelectUserProfile(userID).ConfigureAwait(false);

            if (!userProfile.IsSuccessful)
            {
                userProfile.IsSuccessful = false;
                userProfile.ErrorMessage += ", {failed, _usersDBSelecter.SelectUserProfile}";
                return userProfile;
            }
            else
            {
                userProfile.IsSuccessful = true;
            }

            return userProfile;
        }
    }
}
