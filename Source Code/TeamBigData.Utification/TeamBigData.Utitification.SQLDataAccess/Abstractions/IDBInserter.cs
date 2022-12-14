using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utitification.SQLDataAccess.Abstractions
{
    public interface IDBInserter
    {
        public Task<Response> InsertUser(UserAccount user);
        public Task<Response> InsertUserProfile(UserProfile profile);
    }
}
