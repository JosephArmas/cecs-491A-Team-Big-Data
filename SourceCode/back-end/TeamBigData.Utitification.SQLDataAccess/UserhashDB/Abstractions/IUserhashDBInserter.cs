using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.UserhashDB.Abstractions
{
    public interface IUserhashDBInserter
    {
        public Task<Response> InsertUserHash(String userHash, int userID);
    }
}
