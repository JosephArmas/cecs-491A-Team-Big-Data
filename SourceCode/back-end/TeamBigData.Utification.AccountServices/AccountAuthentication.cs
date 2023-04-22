using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.LogsDB.Abstractions;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;

namespace TeamBigData.Utification.AccountServices
{
    public class AccountAuthentication
    {
        private readonly IUsersDBSelecter _usersDBSelecter;
        //private readonly ILogsDBInserter _logsDBInserter;

        public AccountAuthentication(IUsersDBSelecter usersDBSelecter)//, ILogsDBInserter logsDBInserter)
        {
            _usersDBSelecter = usersDBSelecter;
            //_logsDBInserter = logsDBInserter;
        }

        public async Task<DataResponse<UserAccount>> AuthenticateUserAccount(String email, String password)
        {
            // TODO: Maybe log here and entry point

            var userAccountDataResult = await _usersDBSelecter.SelectUserAccount(email).ConfigureAwait(false);
            if (!userAccountDataResult.isSuccessful)
            {
                userAccountDataResult.isSuccessful = false;
                userAccountDataResult.errorMessage += ", {failed: _usersDBSelecter.SelectUserAccount}";
                return userAccountDataResult;
            }

            var digest = SecureHasher.HashString(userAccountDataResult.data._salt, password);
            if (digest != userAccountDataResult.data._password) 
            {
                userAccountDataResult.isSuccessful = false;
                userAccountDataResult.errorMessage += ", {failed: digest validation}";
                return userAccountDataResult;
            }
            else
            {
                userAccountDataResult.isSuccessful = true;
            }

            return userAccountDataResult;
        }

        public async Task<DataResponse<UserProfile>> AuthenticatedUserProfile(int userID)
        {
            // TODO: Maybe log here and entry point

            var dataResponse = await _usersDBSelecter.SelectUserProfile(userID).ConfigureAwait(false);
            if (!dataResponse.isSuccessful)
            {
                dataResponse.isSuccessful = false;
                dataResponse.errorMessage += ", {failed, _usersDBSelecter.SelectUserProfile}";
                return dataResponse;
            }
            else
            {
                dataResponse.isSuccessful = true;
            }

            return dataResponse;
        }
    }
}
