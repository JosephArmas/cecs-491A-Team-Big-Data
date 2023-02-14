using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;

namespace TeamBigData.Utification.AccountServices
{
    public class AccountRegisterer
    {
        private readonly IDBInserter _dbo;

        public AccountRegisterer(IDBInserter dbo)
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
            Response result1 = new Response();
            Response result2 = new Response();
            result1.isSuccessful = false;
            int userID = 0;
            String username = "";
            String password = encryptor.decryptString(encryptedPassword);
            String salt = "";
            String pepper = "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI";
            var userHash = SecureHasher.HashString(email,pepper);
            UserAccount user = new UserAccount("", "", "", "");
            IDBSelecter selectUsers = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False");
            IDBCounter countSalt = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False");
            if (IsValidPassword(password) && IsValidEmail(email))
            {
                username = GenerateUsername(email);
                //do
                //{
                //   salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(24));
                //}
                //while ((int)countSalt.CountSalt(salt).Result.data > 0);
                var digest = SecureHasher.HashString(password, salt);
                if ((int)selectUsers.SelectLastUserID().data ==0)
                {
                    userID = 1001;
                    user = new UserAccount(userID,username, digest, salt, userHash);
                    result1.data = user._userID;
                }
                else
                {
                    user = new UserAccount(username, digest, salt, userHash);
                    result1.data = user;
                }
                result1 = await _dbo.InsertUser(user).ConfigureAwait(false);

            }
            else if (!IsValidEmail(email))
            {
                result1.errorMessage = "Invalid email provided. Retry again or contact system administrator";
            }
            else if (!IsValidPassword(password))
            {
                result1.errorMessage = "Invalid passphrase provided. Retry again or contact system administrator";
            }

            if (!result1.isSuccessful)
            {
                if (result1.errorMessage.Contains("Violation of PRIMARY KEY"))
                {
                    result1.errorMessage = "Email already linked to an account, please pick a new email";
                }
                else if (result1.errorMessage.Contains("Violation of UNIQUE KEY"))
                {
                    result1.errorMessage = "Unable to assign username. Retry again or contact system administrator";
                }
            }
            else
            {
                result1.errorMessage = "Account created successfully, your username is " + username;
            }
            //If the Error message isn't one of these it return the entire error message from the dbo
            return result1;
        }
    }
}
