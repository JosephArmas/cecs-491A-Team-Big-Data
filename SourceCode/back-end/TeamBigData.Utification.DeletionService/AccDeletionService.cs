using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Pins;
using TeamBigData.Utification.SQLDataAccess.UserhashDB;
using TeamBigData.Utification.SQLDataAccess.UserhashDB.Abstractions;
using TeamBigData.Utification.SQLDataAccess.UsersDB;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;

namespace TeamBigData.Utification.DeletionService
{
    public class AccDeletionService// : IDeletionService
    {
        private readonly IUsersDBSelecter _usersDBSelecter;
        private readonly IUsersDBDeleter _usersDBDeleter;
        private readonly IPinDBDeleter _pinDBDeleter;
        private readonly IUserhashDBUpdater _userhashDBUpdater;
        public AccDeletionService(UsersSqlDAO usersSqlDAO, PinsSqlDAO pinsSqlDAO, UserhashSqlDAO userhashSqlDAO)
        {
            _usersDBSelecter = usersSqlDAO;
            _usersDBDeleter = usersSqlDAO;
            _pinDBDeleter = pinsSqlDAO;
            _userhashDBUpdater = userhashSqlDAO;
        }

        public async Task<Response> DeletePII(String username)
        {
            // Check if real person if so return userhash
            var userAccount = await _usersDBSelecter.SelectUserAccount(username).ConfigureAwait(false);

            if (!userAccount.isSuccessful)
            {
                return new Response(false,userAccount.errorMessage + ", {failed: _usersDBSelecter.SelectUserAccount}");
            }

            // Delete features linked to account
            var response = await _pinDBDeleter.DeletePinsLinkedToUser(userAccount.data._userID).ConfigureAwait(false);

            if (!response.isSuccessful)
            {
                response.isSuccessful = false;
                response.errorMessage += ", {failed: _pinDBDeleter.DeletePinsLinkedToUser}";
                return response;
            }

            // Delete account
            response = await _usersDBDeleter.DeletePIIUserProfile(userAccount.data._userID).ConfigureAwait(false);

            if (!response.isSuccessful)
            {
                response.isSuccessful = false;
                response.errorMessage += ", {failed: _pinDBDeleter.DeletePinsLinkedToUser}";
                return response;
            }

            response = await _usersDBDeleter.DeletePIIUserAccount(userAccount.data._userID).ConfigureAwait(false);

            if (!response.isSuccessful)
            {
                response.isSuccessful = false;
                response.errorMessage += ", {failed: _pinDBDeleter.DeletePinsLinkedToUser}";
                return response;
            }

            // Unlink userhash
            response = await _userhashDBUpdater.UpdateUserID(userAccount.data._userID).ConfigureAwait(false);

            if (!response.isSuccessful)
            {
                response.isSuccessful = false;
                response.errorMessage += ", {failed: _userhashDBUpdater.UpdateUserID}";
                return response;
            }

            response.isSuccessful = true;
            return response;
        }
        /*
        public async Task<Response> DeletePIIFeatures()
        {
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False";
            var err = "User feature data could not be deleted";
            var result = await DeletePII(connectionString, err, 1).ConfigureAwait(false);
            return result;
        }

        public async Task<Response> DeletePIIProfile()
        {
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            var err = "User profile data could not be deleted";
            var result = await DeletePII(connectionString,err,0).ConfigureAwait(false);
            return result;
        }
        public async Task<Response> DeletePII(String connString, String err,int piData)
        {
            var result = new Response();
            result.isSuccessful = false;
            var userDao = new SQLDeletionDAO(connString);
            if (piData == 0)
            {
                result = await userDao.DeleteUser(_userProfile).ConfigureAwait(false);
            }
            else
            {
                result = await userDao.DeleteFeatureInfo(_userProfile).ConfigureAwait(false);
            }
            if (result.isSuccessful == false)
            {
                result.errorMessage = err;
            }
            return result;
        }*/
    }
}