using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.SQLDataAccess.Abstractions;

namespace TeamBigData.Utification.AccountServices
{
    // Not used
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
            result.IsSuccessful = false;
            var updateSql = "Update dbo.Users Set \"disabled\" = 3 where username = '" + username + "';";
            result = await _dbo.Execute(updateSql);
            //Console.WriteLine(result.ErrorMessage);
            if(result.Data is null)
            {
                result.ErrorMessage = "Account doesnt exist";
                return result;
            }
            if ((int)result.Data == 1)
            {
                result.IsSuccessful = true;
                result.ErrorMessage = username + " has been Successfully disabled";
            }
            else
            {
                result.IsSuccessful = false;
                if ((int)result.Data > 1)
                {
                    result.ErrorMessage = "Oops multiple accounts disabled";
                }
                else if ((int)result.Data == 0)
                {
                    result.ErrorMessage = "Account doesnt exist";
                }
            }
            return result;
        }

        public async Task<Response> EnableAccount(String username)
        {
            var result = new Response();
            result.IsSuccessful = false;
            var updateSql = "Update dbo.Users Set \"disabled\" = 0 where username = '" + username + "';";
            result = await _dbo.Execute(updateSql);
            if(result.Data is null)
            {
                result.ErrorMessage = "Account doesnt exist";
                return result;
            }
            if ((int)result.Data == 1)
            {
                result.IsSuccessful = true;
                result.ErrorMessage = username + " has been Successfully enabled";
            }
            else
            {
                result.IsSuccessful = false;
                if ((int)result.Data > 1)
                {
                    result.ErrorMessage = "Oops multiple accounts enabled";
                }
                else if ((int)result.Data == 0)
                {
                    result.ErrorMessage = "Account doesnt exist or wasn't disabled";
                }
            }
            return result;
        }
    }
}