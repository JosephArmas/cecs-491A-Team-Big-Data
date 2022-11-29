using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.ErrorResponse;
using System.Collections;

namespace TeamBigData.Utification.AccountServices
{
    public class AccountAuthenticator
    {
        private readonly IDBCounter _dbo;

        public AccountAuthenticator(IDBCounter dbo)
        {
            _dbo = dbo;
        }

        public async Task<Response> VerifyUser(String tableName, String username, String passwordDigest)
        {
            var result = new Response();
            String[] collumns = { "username", "password" };
            String[] values = { username, passwordDigest };
            result = await _dbo.Count(tableName, "username", collumns, values).ConfigureAwait(false);
            if ((int)result.data > 1)
            {
                result.isSuccessful = false;
                result.errorMessage = "Error: more than 1 user matched";
            }
            else if ((int)result.data == 0)
            {
                result.isSuccessful = false;
                result.errorMessage = "Invalid Username or Password";
            }
            else if ((int)result.data == 1)
            {
                result.isSuccessful = true;
                result.errorMessage = "You have Validated Your Credentials";
            }
            return result;
        }
    }
}
