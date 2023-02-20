using TeamBigData.Utification.ErrorResponse;


namespace TeamBigData.Utification.SQLDataAccess.Abstractions
{
    public interface IDBDeleter
    {
        public Task<Response> DeleteUser(String user);
        public Task<Response> DeleteFeatureInfo(String user);
        public Task<Response> DeleteUserProfile(int userID);
    }
}
