
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.SQLDataAccess.UserhashDB;
using TeamBigData.Utification.SQLDataAccess.UserhashDB.Abstractions;
using TeamBigData.Utification.SQLDataAccess.UsersDB;

namespace TeamBigData.Utification.AccountServices
{
    public class UserhashServices
    {
        private readonly IUserhashDBInserter _userhashDBInserter;
        public UserhashServices(UserhashSqlDAO userhashSqlDAO)
        {
            _userhashDBInserter = userhashSqlDAO;
        }
        public async Task<Response> InsertUserhash(String userhash, int userId)
        {
            var response = await _userhashDBInserter.InsertUserHash(userhash,userId).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.ErrorMessage += ", {failed: _userhashDBInserter.InsertUserHash}";
                return response;
            }
            else
            {
                response.IsSuccessful = true;
                return response;
            }
        }
    }
}
