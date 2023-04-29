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
using TeamBigData.Utification.SQLDataAccess.UsersDB;

namespace TeamBigData.Utification.AccountServices
{
    // Not used
    public class AccountRegisterer
    {
        private readonly IUsersDBInserter _usersDBInserter;
        private readonly IUsersDBSelecter _usersDBSelecter;

        public AccountRegisterer(UsersSqlDAO usersSqlDAO)
        {
            _usersDBInserter = usersSqlDAO;
            _usersDBSelecter = usersSqlDAO;
        }

        public async Task<DataResponse<int>> InsertUserAccount(String email, String password, String userHash)
        {
            DataResponse<int> userID = new DataResponse<int>();

            String salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(24));
            var digest = SecureHasher.HashString(salt, password);

            var response = await _usersDBInserter.InsertUserAccount(email, digest, salt, userHash).ConfigureAwait(false);

            if (!response.isSuccessful)
            {
                userID.errorMessage = response.errorMessage + ", {failed: _usersDBInserter.InsertUser}";
                userID.data = 0;
                return userID;
            }

            var data = await _usersDBSelecter.SelectUserAccount(email).ConfigureAwait(false);

            if (!response.isSuccessful)
            {

                userID.errorMessage = response.errorMessage + ", {failed: _usersDBSelecter.SelectUserAccount}";
                userID.data = 0;
                userID.isSuccessful = false;
            }
            else 
            {
                userID.isSuccessful = true;
                userID.errorMessage = response.errorMessage; 
                userID.data = data.data._userID;
            }

            return userID;
        }

        public async Task<Response> InsertUserProfile(int userId)
        {
            var response = await _usersDBInserter.InsertUserProfile(userId).ConfigureAwait(false);

            if (!response.isSuccessful)
            {
                response.isSuccessful = false;
                response.errorMessage += ", {false: _usersDBInserter.InsertUserProfile}";
            }
            else
            {
                response.isSuccessful = true;
            }

            return response;
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
