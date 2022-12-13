using System.Diagnostics;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.AccountServices;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.Security;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using TeamBigData.Utification.Models;
using System.Collections;

namespace TeamBigData.Utification.ManagerLayer
{
    public class Manager
    {
        private UserProfile? _user;
        private String _otp;
        private DateTime _otpCreated;
        private Boolean _otpVerified;

        public Manager()
        {
            _user = null;
        }

        public Response InsertUser(String email, byte[] encryptedPassword, Encryptor encryptor)
        {
            var response = new Response();
            Stopwatch stopwatch = new Stopwatch();
            response.isSuccessful = false;
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            IDBUserInserter sqlDAO = new SqlDAO(connectionString);
            var accountManager = new AccountRegisterer(sqlDAO);
            stopwatch.Start();
            response = accountManager.InsertUser(email, encryptedPassword, encryptor).Result;
            stopwatch.Stop();
            Log log;
            var logger = new Logger(new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            if (response.isSuccessful)
            {
                String username = response.errorMessage.Substring(47);
                if (stopwatch.ElapsedMilliseconds > 5000)
                {
                    log = new Log(1, "Warning", email, "Manager.InsertUser()", "Data", "Account Registration Took Longer Than 5 Seconds");
                }
                else
                {
                    log = new Log(1, "Info", email, "Manager.InsertUser()", "Data", "Account Registration Succesful");
                }
            }
            else
            {
                log = new Log(1, "Error", email, "Manager.InsertUser()", "Data", "Error in Creating Account");
            }
            logger.Log(log);
            var result2 = sqlDAO.InsertUserProfile(new UserProfile(email)).Result;
            return response;
        }

        public Response VerifyUser(String username, byte[] encryptedPassword, Encryptor encryptor)
        {
            var result = new Response();
            if(_user == null)
            {
                var logConnectionString = @"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True";
                var logDAO = new SqlDAO(logConnectionString);
                ILogger logger = new Logger(logDAO);
                Log log;
                var hasher = new SecureHasher();
                var password = encryptor.decryptString(encryptedPassword);
                if(!AccountRegisterer.IsValidPassword(password) || !AccountRegisterer.IsValidEmail(username))
                {
                    result.isSuccessful = false;
                    result.errorMessage = "Invalid username or password provided. Retry again or contact system administrator";
                    return result;
                }
                var hashedPassword = SecureHasher.HashString(username, password);
                var user = new UserAccount(username, hashedPassword);
                var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
                var userDao = new SqlDAO(connectionString);
                result = userDao.GetUser(user).Result;
                if(result.isSuccessful)
                {
                    _user = result.data as UserProfile;
                    var hash = SecureHasher.HashString(DateTime.Now.Ticks, username);
                    _otp = hash.Substring(0, 16).Replace("-", "");
                    _otpCreated = DateTime.Now;
                    Console.WriteLine("Please enter the OTP to finish Authentication");
                    Console.WriteLine(_otp);
                    log = new Log(2, "Info", username, "Authentication", "Data", "Successfull Logged In");
                    logger.Log(log);
                    result.isSuccessful = true;
                }
                else
                {
                    if(result.errorMessage.Equals("Error: Invalid Username or Password"))
                    {
                        log = new Log(2, "Warning", username, "Authentication", "Data", "Insuccessful Log In Attempt");
                        logger.Log(log);
                        result.isSuccessful = false;
                        result.errorMessage = "Invalid username or password provided. Retry again or contact system administrator";
                        var attemptsResult = logDAO.GetLoginAttempts(username).Result;
                        var attempts = attemptsResult.data as ArrayList;
                        if (attemptsResult.isSuccessful)
                        {
                            int n = attempts.Count;
                            int i = 0;
                            int failedAttempts = 0;
                            while (i < n)
                            {
                                if (attempts[i].Equals("Warning"))
                                {
                                    failedAttempts++;
                                }
                                else
                                {
                                    failedAttempts = 0;
                                }
                                i++;
                            }
                            if (failedAttempts > 2)
                            {
                                var disabler = new AccountDisabler(userDao);
                                disabler.DisableAccount(username);
                                result.errorMessage = "You have failed to login 3 times in 24 hours, your account will now be disabled";
                            }
                        }
                    }
                }
            }
            else
            {
                result.isSuccessful = false;
                result.errorMessage = "Error You are already Logged In";
            }
            return result;
        }

        public Response VerifyOTP(String enteredOTP)
        {
            var result = new Response();
            var currentTime = DateTime.Now;
            if (enteredOTP.Equals(_otp))
            {
                if ((currentTime.Ticks - _otpCreated.Ticks) < 1200000000) //1200000000 ticks in 2 minutes
                {
                    result.isSuccessful = true;
                    result.errorMessage = "You have successfully logged in";
                    result.data = _user;
                    _otpVerified = true;
                }
                else
                {
                    result.isSuccessful = false;
                    result.errorMessage = "You did not enter in the OTP within 2 minutes, Please try again";
                }
            }
            else
            {
                result.isSuccessful = false;
                result.errorMessage = "Error: The Entered in OTP doesn't match";
            }
            return result;
        }

        public Response LogOut()
        {
            var response = new Response();
            if(_user != null)
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

        public bool IsAuthenticated()
        {
            if (_user is null)
            {
                return false;
            }
            else
            {
                return _otpVerified;
            }
        }
    }
}