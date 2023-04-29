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
        private readonly IUsersDBUpdater _usersDBUpdater;

        public AccountAuthentication(UsersSqlDAO usersSqlDAO)
        {
            _usersDBSelecter = usersSqlDAO;
            _usersDBUpdater = usersSqlDAO;
        }

        public async Task<DataResponse<UserAccount>> AuthenticateUserAccount(String email, String password)
        {
            var userAccount = await _usersDBSelecter.SelectUserAccount(email).ConfigureAwait(false);

            if (!userAccount.isSuccessful)
            {
                userAccount.isSuccessful = false;
                userAccount.errorMessage += ", {failed: _usersDBSelecter.SelectUserAccount}";
                return userAccount;
            }

            if (!userAccount.data._verified)
            {
                userAccount.isSuccessful = false;
                userAccount.errorMessage = "Error: Account disabled. Perform account recovery or contact system admin";
                userAccount.data = new UserAccount();
                return userAccount;
            }

            var digest = SecureHasher.HashString(userAccount.data._salt, password);

            if (digest != userAccount.data._password) 
            {
                userAccount.isSuccessful = false;
                userAccount.errorMessage += ", {failed: digest validation}";

                var response = await _usersDBUpdater.IncrementUserDisabled(userAccount.data._userID).ConfigureAwait(false);
                if (!response.isSuccessful)
                {
                    userAccount.isSuccessful = false;
                    userAccount.errorMessage = response.errorMessage;
                    return userAccount;
                }
            }
            else
            {
                userAccount.isSuccessful = true;
            }

            return userAccount;
        }

        public async Task<DataResponse<UserProfile>> AuthenticatedUserProfile(int userID)
        {
            var userProfile = await _usersDBSelecter.SelectUserProfile(userID).ConfigureAwait(false);

            if (!userProfile.isSuccessful)
            {
                userProfile.isSuccessful = false;
                userProfile.errorMessage += ", {failed, _usersDBSelecter.SelectUserProfile}";
                return userProfile;
            }
            else
            {
                userProfile.isSuccessful = true;
            }

            return userProfile;
        }
    }
}
