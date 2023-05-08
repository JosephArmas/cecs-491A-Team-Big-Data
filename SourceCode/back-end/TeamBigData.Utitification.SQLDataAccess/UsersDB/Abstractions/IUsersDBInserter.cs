using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions
{
   public interface IUsersDBInserter
    {
        public Task<Response> InsertUserAccount(String email, String digest, String salt, String userhash);

        public Task<Response> InsertUserProfile(int userId);

        public Task<Response> InsertRecoveryRequest(int userID, String digest, String salt);
    }
}
