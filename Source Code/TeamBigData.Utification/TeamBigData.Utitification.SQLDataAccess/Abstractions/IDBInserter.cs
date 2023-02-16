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
        public Response InsertUser(UserAccount user);
        public Response InsertUserProfile(UserProfile profile);
        public Response InsertUserHash(String userHash, int userID);
    }
}
