
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.SQLDataAccess.UserhashDB.Abstractions;

namespace TeamBigData.Utification.AccountServices
{
    public class UserhashServices
    {
        private readonly IUserhashDBInserter _userhashDBInserter;
        public UserhashServices(IUserhashDBInserter userhashDBInserter)
        {
            _userhashDBInserter = userhashDBInserter;
        }
        public async Task<Response> InsertUserhash(String userhash, int userId)
        {
            var response = await _userhashDBInserter.InsertUserHash(userhash,userId).ConfigureAwait(false);

            if (!response.isSuccessful)
            {
                response.isSuccessful = false;
                response.errorMessage += ", {failed: _userhashDBInserter.InsertUserHash}";
                return response;
            }
            else
            {
                response.isSuccessful = true;
                return response;
            }
        }
    }
}
