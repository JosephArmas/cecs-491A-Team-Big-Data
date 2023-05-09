
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.SQLDataAccess.DTO;
using TeamBigData.Utification.SQLDataAccess.UsersDB;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;

namespace TeamBigData.Utification.AccountServices
{
    public class RecoveryServices
    {
        private readonly IUsersDBSelecter _usersDBSelecter;
        private readonly IUsersDBInserter _userDBInserter;
        private readonly IUsersDBUpdater _userDBUpdater;

        public RecoveryServices(UsersSqlDAO usersSqlDAO)
        {
            _usersDBSelecter = usersSqlDAO;
            _userDBInserter = usersSqlDAO;
            _userDBUpdater = usersSqlDAO;
        }

        public async Task<DataResponse<int>> GetUserIDbyName(String username)
        {
            return await _usersDBSelecter.SelectUserID(username);
        }

        public async Task<Response> RequestRecoveryNewPassword(String username, String password)
        {
            // validate is a user
            var userAccount = await _usersDBSelecter.SelectUserAccount(username).ConfigureAwait(false);

            if (!userAccount.IsSuccessful)
            {
                return new Response(false, userAccount.ErrorMessage + ", {failed: _usersDBSelecter.SelectUserAccount}");
            }

            // Make salt and digest
            String salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(24));
            var digest = SecureHasher.HashString(salt, password);

            // send a recovery request
            var response = await _userDBInserter.InsertRecoveryRequest(userAccount.Data.UserID, digest, salt).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                return new Response(false, response.ErrorMessage + ", {failed: _userDBInserter.InsertRecoveryRequest}");
            }
            else
            {
                return new Response(true, response.ErrorMessage);
            }
        }

        public async Task<DataResponse<List<RecoveryRequests>>> GetRecoveryRequestsTable()
        {
            var dataResponse = await _usersDBSelecter.SelectRecoveryRequestsTable().ConfigureAwait(false);

            if (!dataResponse.IsSuccessful)
            {
                dataResponse.IsSuccessful = false;
                dataResponse.ErrorMessage += ", {failed: _usersDBSelecter.SelectRecoveryRequestsTable}";
            }
            else
            {
                dataResponse.IsSuccessful = true;
            }

            return dataResponse;
        }

        public async Task<DataResponse<ValidRecovery>> GetNewPassword(int userID)
        {
            var validRecovery = await _usersDBSelecter.SelectRecoveryUser(userID).ConfigureAwait(false);

            if (!validRecovery.IsSuccessful)
            {
                validRecovery.IsSuccessful = false;
                validRecovery.ErrorMessage += ", {failed: _usersDBSelecter.SelectRecoveryUser}"; 
                return validRecovery;
            }
            
            var response = await _userDBUpdater.UpdateRecoveryFulfilled(userID).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                validRecovery.IsSuccessful = false;
                validRecovery.ErrorMessage += ", {failed: _userDBUpdater.UpdateRecoveryFulfilled}";
                return validRecovery;
            }
            else
            {
                validRecovery.IsSuccessful = true;
            }

            return validRecovery;
        }

        public async Task<Response> SaveNewPassword(int userID, String password, String salt)
        {
            var response = await _userDBUpdater.UpdateUserPassword(userID, password, salt).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.ErrorMessage += ", {failed: _userDBUpdater.UpdateUserPassword}";
                return response;
            }
            else
            {
                response.IsSuccessful = true;
            }

            return response;
        }
    }
}
