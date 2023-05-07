using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Pins;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB;
using TeamBigData.Utification.SQLDataAccess.UserhashDB;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;
using TeamBigData.Utification.SQLDataAccess.UsersDB;
using TeamBigData.Utification.SQLDataAccess.UserhashDB.Abstractions;

namespace TeamBigData.Utification.DeletionService
{
    public class AccDeletionService : IDeletionService
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

            if (!userAccount.IsSuccessful)
            {
                return new Response(false, userAccount.ErrorMessage + ", {failed: _usersDBSelecter.SelectUserAccount}");
            }

            // Delete features linked to account
            var response = await _pinDBDeleter.DeletePinsLinkedToUser(userAccount.Data.UserID).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.ErrorMessage += ", {failed: _pinDBDeleter.DeletePinsLinkedToUser}";
                return response;
            }

            // Delete account
            response = await _usersDBDeleter.DeletePIIUserProfile(userAccount.Data.UserID).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.ErrorMessage += ", {failed: _pinDBDeleter.DeletePinsLinkedToUser}";
                return response;
            }

            response = await _usersDBDeleter.DeletePIIUserAccount(userAccount.Data.UserID).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.ErrorMessage += ", {failed: _pinDBDeleter.DeletePinsLinkedToUser}";
                return response;
            }

            // Unlink userhash
            response = await _userhashDBUpdater.UnlinkUserhashFrom(userAccount.Data.UserID).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.ErrorMessage += ", {failed: _userhashDBUpdater.UpdateUserID}";
                return response;
            }

            response.IsSuccessful = true;
            return response;
        }
    }
}