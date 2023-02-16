using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.Abstractions
{
    public interface IDBDeleter
    {
        public Task<Response> DeleteUserProfile(int userID);
    }
}
