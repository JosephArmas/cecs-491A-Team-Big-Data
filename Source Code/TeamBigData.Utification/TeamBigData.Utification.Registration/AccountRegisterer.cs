using System.Text.RegularExpressions;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Security;
using TeamBigData.Utification.SQLDataAccess;

namespace TeamBigData.Utification.AccountServices
{
    public class AccountRegisterer
    {
        private readonly IDBUserInserter _dbo;

        public AccountRegisterer(IDBUserInserter dbo)
        {
            _dbo = dbo;
        }

        public static bool IsValidPassword(String password)
        {
            Regex passwordAllowedCharacters = new Regex(@"^[a-zA-Z0-9@.,!\s-]*$");
            if (passwordAllowedCharacters.IsMatch(password) && password.Length >= 8)
                return true;
            else
                return false;
        }

        public static bool IsValidEmail(String email)
        {
            Regex emailAllowedCharacters = new Regex(@"^[a-zA-Z0-9@.-]*$");
            if (emailAllowedCharacters.IsMatch(email) && email.Contains('@') && (!email.StartsWith("@")))
                return true;
            else
                return false;
        }

        public static String GenerateUsername(String email)
        {
            return email;
        }

        public async Task<Response> InsertUser(String email, byte[] encryptedPassword, Encryptor encryptor)
        {
            Response result = new Response();
            result.isSuccessful = false;
            String username = "";
            String password = encryptor.decryptString(encryptedPassword);
            if (IsValidPassword(password) && IsValidEmail(email))
            {
                username = GenerateUsername(email);
                var digest = SecureHasher.HashString(username, password);
                var user = new UserAccount(username, digest);
                result = await _dbo.InsertUser(user).ConfigureAwait(false);
            }
            else if (!IsValidEmail(email))
            {
                result.errorMessage = "Invalid email provided. Retry again or contact system administrator";
            }
            else if (!IsValidPassword(password))
            {
                result.errorMessage = "Invalid passphrase provided. Retry again or contact system administrator";
            }

            if (!result.isSuccessful)
            {
                if (result.errorMessage.Contains("Violation of PRIMARY KEY"))
                {
                    result.errorMessage = "Email already linked to an account, please pick a new email";
                }
                else if (result.errorMessage.Contains("Violation of UNIQUE KEY"))
                {
                    result.errorMessage = "Unable to assign username. Retry again or contact system administrator";
                }
            }
            else
            {
                result.errorMessage = "Account created successfully, your username is " + username;
            }
            //If the Error message isn't one of these it return the entire error message from the dbo
            return result;
        }
    }
}
