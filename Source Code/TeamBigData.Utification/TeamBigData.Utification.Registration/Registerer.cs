using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.ErrorResponse;
using System.Text.RegularExpressions;

namespace TeamBigData.Utification.Registration
{
    public class Registerer
    {
        private readonly IDBInserter _dbo;

        public Registerer(IDBInserter dbo)
        {
            _dbo = dbo;
        }
        
        public static bool IsValidUsername(String username)
        {
            Regex usernameAllowedCharacters = new Regex(@"^[a-zA-Z0-9@.-]*$");
            if (usernameAllowedCharacters.IsMatch(username) && username.Length >= 8)
                return true;
            else
                return false;
        }
        
        public static bool IsValidPassword(String password)
        {
            Regex passwordAllowedCharacters = new Regex(@"^[a-zA-Z0-9@.,!-]*$");
            if (passwordAllowedCharacters.IsMatch(password) && password.Length >= 8)
                return true;
            else
                return false;
        }

        public static bool IsValidEmail(String email)
        {
            Regex emailAllowedCharacters = new Regex(@"^[a-zA-Z0-9@.-]*$");
            if (emailAllowedCharacters.IsMatch(email) && email.Contains('@'))
                return true;
            else
                return false;
        }

        public Response InsertUser(String tableName, String username, String password, String email)
        {
            Response result = new Response();
            result.isSuccessful = false;
            if (IsValidUsername(username) && IsValidPassword(password) && IsValidEmail(email))
            {
                String[] values = {username, password, email};
                result = _dbo.Insert(tableName, values);
            }
            else if(!IsValidEmail(email))
            {
                result.errorMessage = "Invalid email provided. Retry again or contact system administrator";
            }
            else if(!IsValidPassword(password))
            {
                result.errorMessage = "Invalid passphrase provided. Retry again or contact system administrator";
            }
            else
            {
                result.errorMessage = "Invalid username provided. Retry again or contact system administrator";
            }
            if (result.errorMessage.Contains("Violation of PRIMARY KEY"))
            {
                result.errorMessage = "Email already linked to an account, please pick a new email";
            }
            else if (result.errorMessage.Contains("Violation of UNIQUE KEY"))
            {
                result.errorMessage = "Username taken, please pick a new username";
            }
            //If the Error message isn't one of these it return the entire error message from the dbo
            return result;
        }
    }
}
