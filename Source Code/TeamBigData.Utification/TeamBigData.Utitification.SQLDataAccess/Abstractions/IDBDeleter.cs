using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.Abstractions
{
    public interface IDBDeleter
    {
        public Task<Response> DeleteUser(UserProfile user);
        public Task<Response> DeleteFeatureInfo(UserProfile user);
        //public Task<Response> DeleteUserProfile(int userID);
    }
}
