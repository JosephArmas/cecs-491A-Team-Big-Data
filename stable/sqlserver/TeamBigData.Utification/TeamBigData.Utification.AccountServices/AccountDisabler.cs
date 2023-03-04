using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.SQLDataAccess.Abstractions;

namespace TeamBigData.Utification.AccountServices
{
    public class AccountDisabler
    {
        private readonly IDAO _dbo;

        public AccountDisabler(IDAO dbo)
        {
            _dbo = dbo;
        }

        public async Task<Response> DisableAccount(String username)
        {
            var result = new Response();
            result.isSuccessful = false;
            var updateSql = "Update dbo.Users Set \"disabled\" = 1 where username = '" + username + "';";
            result = await _dbo.Execute(updateSql);
            if ((int)result.data == 1)
            {
                result.isSuccessful = true;
                result.errorMessage = username + " has been Successfully disabled";
            }
            else
            {
                result.isSuccessful = false;
                if ((int)result.data > 1)
                {
                    result.errorMessage = "Oops multiple accounts disabled";
                }
                else if ((int)result.data == 0)
                {
                    result.errorMessage = "Account doesnt exist";
                }
            }
            return result;
        }

        public async Task<Response> EnableAccount(String username)
        {
            var result = new Response();
            result.isSuccessful = false;
            var updateSql = "Update dbo.Users Set \"disabled\" = 0 where username = '" + username + "';";
            result = await _dbo.Execute(updateSql);
            if ((int)result.data == 1)
            {
                result.isSuccessful = true;
                result.errorMessage = username + " has been Successfully enabled";                
            }
            else
            {
                result.isSuccessful = false;
                if ((int)result.data > 1)
                {
                    result.errorMessage = "Oops multiple accounts enabled";
                }
                else if ((int)result.data == 0)
                {
                    result.errorMessage = "Account doesnt exist or wasn't disabled";
                }
            }
            return result;
        }
    }
}