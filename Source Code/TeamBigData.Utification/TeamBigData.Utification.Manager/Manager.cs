using System.Diagnostics;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.AccountServices;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.Security;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace TeamBigData.Utification.ManagerLayer
{
    public class Manager
    {
        private UserAccount? _user;

        public Manager()
        {
            _user = null;
        }
        public Response InsertUser(String email, String password)
        {
            var response = new Response();
            Stopwatch stopwatch = new Stopwatch();
            response.isSuccessful = false;
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            IDBInserter sqlDAO = new SqlDAO(connectionString);
            var accountManager = new AccountRegisterer(sqlDAO);
            stopwatch.Start();
            response = accountManager.InsertUser("dbo.Users", email, password).Result;
            stopwatch.Stop();
            String insertSql;
            var logger = new Logger(new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            if (response.isSuccessful)
            {
                String username = response.errorMessage.Substring(47);
                if (stopwatch.ElapsedMilliseconds > 5000)
                {
                    insertSql = "INSERT INTO dbo.Logs (CorrelationID,LogLevel,[User],[DateTime],[Event],Category,[Message]) VALUES (1, 'Warning', '" + email + "' ,'" + DateTime.UtcNow.ToString() + "', 'Manager.InsertUser()', 'Data', 'Account Registration Took Longer than 5 Seconds')";
                }
                else
                {
                    insertSql = "INSERT INTO dbo.Logs (CorrelationID,LogLevel,[User],[DateTime],[Event],Category,[Message]) VALUES (1, 'Info', '" + email + "' ,'" + DateTime.UtcNow.ToString() + "', 'Manager.InsertUser()', 'Data', 'Account Registration Successful')";
                }
            }
            else
            {
                insertSql = "INSERT INTO dbo.Logs (CorrelationID,LogLevel,[User],[DateTime],[Event],Category,[Message]) VALUES (1, 'Error', '" + email + "' ,'" + DateTime.UtcNow.ToString() + "', 'Manager.InsertUser()', 'Data', 'Error in Creating Account')";
            }
            var logRes = logger.Log(insertSql);
            return response;
        }

        public Response VerifyUser(String username, byte[] encryptedPassword, Encryptor encryptor)
        {
            var result = new Response();
            if(_user == null || !_user.IsVerified())
            {
                var hasher = new SecureHasher();
                var password = encryptor.decryptString(encryptedPassword);
                var hashedPassword = SecureHasher.HashString(username, password);
                var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
                IDBCounter testDBO = new SqlDAO(connectionString);
                var authenticator = new AccountAuthenticator(testDBO);
                result = authenticator.VerifyUser("dbo.Users", username, hashedPassword).Result;
                if (result.isSuccessful)
                {
                    _user = new UserAccount(username);
                }
            }
            else
            {
                result.isSuccessful = false;
                result.errorMessage = "Error You are already Logged In";
            }
            return result;
        }

        public Response LogOut()
        {
            var response = new Response();
            if(_user != null && _user.IsVerified())
            {
                _user = null;
                response.isSuccessful = true;
                response.errorMessage = "You have been Successfully Logged Out";
            }
            else
            {
                response.isSuccessful = false;
                response.errorMessage = "Error you are not logged in";
            }
            return response;
        }

        public String SendOTP()
        {
            if(_user != null)
            {
                return _user.GetOTP();
            }
            else
            {
                return "Error No User";
            }
        }

        public Response ReceiveOTP(String otp)
        {
            var result = new Response();
            result.isSuccessful = false;
            if(_user != null)
            {
                result = _user.VerifyOTP(otp);
            }
            else
            {
                result.isSuccessful = false;
                result.errorMessage = "Error Please Verify your Credentials before verifying your OTP";
            }
            return result;
        }

        public bool IsAuthenticated()
        {
            if(_user == null)
            {
                return false;
            }
            else
            {
                return _user.IsVerified();
            }
        }
    }
}