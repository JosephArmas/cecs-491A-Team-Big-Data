
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
            // TODO: Maybe log here and entry point

            var result = await _userhashDBInserter.InsertUserHash(userhash,userId).ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                result.isSuccessful = false;
                result.errorMessage += ", {failed: _userhashDBInserter.InsertUserHash}";
                return result;
            }
            else
            {
                result.isSuccessful = true;
                return result;
            }
        }
    }
}
