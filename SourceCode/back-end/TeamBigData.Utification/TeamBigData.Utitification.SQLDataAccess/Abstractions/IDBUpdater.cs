using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.Abstractions
{
    public interface IDBUpdater
    {
        public Task<Response> UpdateUserProfile(UserProfile user);
    }
}
