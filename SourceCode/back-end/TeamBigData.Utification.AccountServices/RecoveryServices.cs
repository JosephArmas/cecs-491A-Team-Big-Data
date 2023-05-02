
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.SQLDataAccess.DTO;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;

namespace TeamBigData.Utification.AccountServices
{
    public class RecoveryServices
    {
        private readonly IUsersDBSelecter _usersDBSelecter;
        private readonly IUsersDBInserter _userDBInserter;
        private readonly IUsersDBUpdater _userDBUpdater;

        public RecoveryServices(IUsersDBSelecter usersDBSelecter, IUsersDBInserter usersDBInserter, IUsersDBUpdater usersDBUpdater)
        {
            _usersDBSelecter = usersDBSelecter;
            _userDBInserter = usersDBInserter;
            _userDBUpdater = usersDBUpdater;
        }

        public async Task<Response> RequestRecoveryNewPassword(String username, String password)
        {
            // validate is a user
            var userAccount = await _usersDBSelecter.SelectUserAccount(username).ConfigureAwait(false);

            if (!userAccount.isSuccessful)
            {
                return new Response(false, userAccount.errorMessage + ", {failed: _usersDBSelecter.SelectUserAccount}");
            }

            // Make salt and digest
            String salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(24));
            var digest = SecureHasher.HashString(salt, password);

            // send a recovery request
            var response = await _userDBInserter.InsertRecoveryRequest(userAccount.data._userID, digest, salt).ConfigureAwait(false);

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

            if (!dataResponse.isSuccessful)
            {
                dataResponse.isSuccessful = false;
                dataResponse.errorMessage += ", {failed: _usersDBSelecter.SelectRecoveryRequestsTable}";
            }
            else
            {
                dataResponse.isSuccessful = true;
            }

            return dataResponse;
        }

        public async Task<DataResponse<ValidRecovery>> GetNewPassword(int userID)
        {
            var validRecovery = await _usersDBSelecter.SelectRecoveryUser(userID).ConfigureAwait(false);

            if (!validRecovery.isSuccessful)
            {
                validRecovery.isSuccessful = false;
                validRecovery.errorMessage += ", {failed: _usersDBSelecter.SelectRecoveryUser}"; 
                return validRecovery;
            }
            
            var response = await _userDBUpdater.UpdateRecoveryFulfilled(userID).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                validRecovery.isSuccessful = false;
                validRecovery.errorMessage += ", {failed: _userDBUpdater.UpdateRecoveryFulfilled}";
                return validRecovery;
            }
            else
            {
                validRecovery.isSuccessful = true;
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
