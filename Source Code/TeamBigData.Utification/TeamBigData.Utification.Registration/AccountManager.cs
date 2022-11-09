using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Security;

namespace TeamBigData.Utification.Registration
{
    public class AccountManager
    {
        private readonly IDBInserter _dbo;

        public AccountManager(IDBInserter dbo)
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
            String username = email.Remove(email.LastIndexOf('@'));
            Random rng = new Random();
            int randomNumber = rng.Next(10000);
            username += "-";
            if (randomNumber < 10)
            {
                username = username + "000";
            }
            else if (randomNumber < 100)
            {
                username += "00";
            }
            else if (randomNumber < 1000)
            {
                username += "0";
            }
            username += randomNumber.ToString();
            if (username.Length < 8)
            {
                username += "--";
            }
            return username;
        }

        public async Task<Response> InsertUser(String tableName, String email, String password)
        {
            Response result = new Response();
            result.isSuccessful = false;
            String username = "";
            if (IsValidPassword(password) && IsValidEmail(email))
            {
                username = GenerateUsername(email);
                String[] values = { username, SecureHasher.HashString(password), email };
                result = await _dbo.Insert(tableName, values).ConfigureAwait(false);
            }
            else if(!IsValidEmail(email))
            {
                result.errorMessage = "Invalid email provided. Retry again or contact system administrator";
            }
            else if(!IsValidPassword(password))
            {
                result.errorMessage = "Invalid passphrase provided. Retry again or contact system administrator";
            }

            if(!result.isSuccessful)
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
