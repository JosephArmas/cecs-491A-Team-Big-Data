using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
namespace TeamBigData.Utification.SQLDataAccess.Abstractions
{
    public interface IDBInserter
    {
        public Task<Response> InsertUser(String email, String digest, String salt, String userhash);
        public Task<Response> InsertUserProfile(int userId);
        public Task<Response> InsertUserHash(String userHash, int userID);
        public Task<Response> IncrementUserAccountDisabled(UserAccount userAccount);
    }
}
