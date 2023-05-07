using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions
{
    public interface IUsersDBDeleter
    {
        public Task<Response> DeletePIIUserProfile(int userID);
        public Task<Response> DeletePIIUserAccount(int userID);
    }
}
