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
        private String? _username;

        public Manager()
        {
            _username = null;
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

        public Response AuthenticateUser(String username, byte[] encryptedPassword, Encryptor encryptor)
        {
            var result = new Response();
            if(_username == null)
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
                    /*var otpResult = ReceiveOTP().Result;
                    if (otpResult.isSuccessful)
                    {
                        _username = username;
                    }
                    result = otpResult;*/
                    _username = username;
                }
            }
            else
            {
                result.isSuccessful = false;
                result.errorMessage = "Error You are already Logged In";
            }
            return result;
        }

        public Response LogOut(String username)
        {
            var response = new Response();
            if(_username != null)
            {
                if (_username.Equals(username))
                {
                    _username = null;
                    response.isSuccessful = true;
                    response.errorMessage = "You have been Successfully Logged Out";
                }
                else
                {
                    response.isSuccessful = false;
                    response.errorMessage = "Error your username doesn't match";
                }
            }
            else
            {
                response.isSuccessful = false;
                response.errorMessage = "Error you are not logged in";
            }
            return response;
        }
/*
        public Task<Response> ReceiveOTP()
        {

            var tcs = new TaskCompletionSource<Response>();
            var result = new Response();
            result.isSuccessful = false;
            var otp = "";
            var rng = new Random();
            for(int i = 0; i < 8; i++)
            {
                var random = rng.Next(0, 61);//62 valid characters 10 digits + 26 lowercase + 26 uppercase letters
                if(random < 10) //offsett the numbers to get the ascii values
                {
                    random += 48; //digits start at 48
                }
                else if(random > 10 && random < 36)
                {
                    random += 55; //uppercase starts at 65 - 10 = 55 offset
                }
                else
                {
                    random += 61; //lowercase starts at 97 - 36 = 61 offset
                }
                otp += (char)random;
            }
            Console.WriteLine("Please enter " + otp);
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var enteredOtp = Console.ReadLine();
            watch.Stop();
            if(watch.ElapsedMilliseconds > 120000)
            {
                result.isSuccessful = false;
                result.errorMessage = "You took to long to enter the One Time Password, Please Try Again";
            }
            else
            {
                if (enteredOtp.Equals(otp))
                {
                    result.isSuccessful = true;
                    result.errorMessage = "You are Successfully Authenticated";
                }
                else
                {
                    result.isSuccessful = false;
                    result.errorMessage = "Incorrect OTP entered";
                }
            }
            tcs.SetResult(result);
            return tcs.Task;
        }
*/
    }
}