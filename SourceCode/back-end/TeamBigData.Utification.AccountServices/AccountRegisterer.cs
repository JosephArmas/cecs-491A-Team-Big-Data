using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Security.Principal;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;
using TeamBigData.Utification.SQLDataAccess.LogsDB.Abstractions;

namespace TeamBigData.Utification.AccountServices
{
    // Not used
    public class AccountRegisterer
    {
        private readonly IUsersDBInserter _usersDBInserter;
        private readonly IUsersDBSelecter _usersDBSelecter;
        //private readonly ILogsDBInserter _logsDBInserter;

        public AccountRegisterer(IUsersDBInserter usersDBInserter, IUsersDBSelecter usersDBSelecter)//, ILogsDBInserter logsDBInserter)
        {
            _usersDBInserter = usersDBInserter;
            _usersDBSelecter = usersDBSelecter;
            //_logsDBInserter = logsDBInserter;
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

        public async Task<DataResponse<int>> InsertUserAccount(String email, String password, String userHash)
        {
            // TODO: Maybe log here and entry point

            DataResponse<int> dataResponse = new DataResponse<int>();

            String salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(24));
            var digest = SecureHasher.HashString(salt, password);

            var result = await _usersDBInserter.InsertUserAccount(email, digest, salt, userHash).ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                dataResponse.errorMessage = result.errorMessage + ", {failed: _usersDBInserter.InsertUser}";
                dataResponse.data = 0;
                return dataResponse;
            }

            var data = await _usersDBSelecter.SelectUserAccount(email).ConfigureAwait(false);
            if (!result.isSuccessful)
            {

                dataResponse.errorMessage = result.errorMessage + ", {failed: _usersDBSelecter.SelectUserAccount}";
                dataResponse.data = 0;
                dataResponse.isSuccessful = false;
            }
            else 
            {
                dataResponse.isSuccessful = true;
                dataResponse.errorMessage = result.errorMessage; 
                dataResponse.data = data.data._userID;
            }

            return dataResponse;
        }

        public async Task<Response> InsertUserProfile(int userId)
        {
            // TODO: Maybe log here and entry point
            //Log log;
            var result = await _usersDBInserter.InsertUserProfile(userId).ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                result.isSuccessful = false;
                result.errorMessage += ", {false: _usersDBInserter.InsertUserProfile}";
            }
            else
            {
                result.isSuccessful = true;
            }
            return result;
        }

        //TODO: fix admin creation
        /*
        public async Task<Response> InsertUserAdmin(String email, byte[] encryptedPassword, Encryptor encryptor, UserProfile userProfile)
        {
            Response result = new Response();
            result.isSuccessful = false;
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                result.isSuccessful = false;
                result.errorMessage = "Unauthorized access to Admin Creation";
                return result;
            }
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
        */
    }
}
