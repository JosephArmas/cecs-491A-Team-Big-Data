using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.AccountServices;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess
{
    public interface IDBUserInserter
    {
        public Task<Response> InsertUser(UserAccount user);

        public Task<Response> InsertUserProfile(UserProfile profile);
    }
}
