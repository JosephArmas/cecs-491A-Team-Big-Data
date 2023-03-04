using System.Collections;
using System.Diagnostics;
using System.Security.Principal;
using System.Text;
using TeamBigData.Utification.AccountServices;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;

namespace TeamBigData.Utification.Manager
{
    public class SecurityManager
    {
        private UserProfile? _user;
        private String _otp;
        private DateTime _otpCreated;
        private Boolean _otpVerified;

        public SecurityManager()
        {
            _user = null;
        }
        public Response InsertUser(String email, byte[] encryptedPassword, Encryptor encryptor)
        {
            var response = new Response();
            Stopwatch stopwatch = new Stopwatch();
            response.isSuccessful = false;
            //var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            var connectionString = @"Server=.;Database=TeamBigData.Utification.Users;Uid=root;Pwd=root;TrustServerCertificate=True;Encrypt=False";
            IDBInserter sqlDAO = new SqlDAO(connectionString);
            var accountManager = new AccountRegisterer(sqlDAO);
            stopwatch.Start();
            response = accountManager.InsertUser(email, encryptedPassword, encryptor).Result;
            stopwatch.Stop();
            Log log;
            var logger = new Logger(new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            if (response.isSuccessful)
            {
                sqlDAO.InsertUserProfile(new UserProfile(email, "Regular User"));
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
            return response;
        }

        public async Task<Response> VerifyUser(String username, byte[] encryptedPassword, Encryptor encryptor)
        {
            var tcs = new TaskCompletionSource<Response>();
            var result = new Response();
            if (_user == null)
            {
                var logConnectionString = @"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True";
                var logDAO = new SqlDAO(logConnectionString);
                ILogger logger = new Logger(logDAO);
                Log log;
                var hasher = new SecureHasher();
                var password = encryptor.decryptString(encryptedPassword);
                if (!AccountRegisterer.IsValidPassword(password) || !AccountRegisterer.IsValidEmail(username))
                {
                    result.isSuccessful = false;
                    result.errorMessage = "Invalid username or password provided. Retry again or contact system administrator";
                    return result;
                }
                var hashedPassword = SecureHasher.HashString(username, password);
                var user = new UserAccount(username, hashedPassword);
                var connectionString = @"Server=.;Database=TeamBigData.Utification.Users;Uid=root;Pwd=root;;TrustServerCertificate=True;Encrypt=False";
                var userDao = new SqlDAO(connectionString);
                result = await userDao.GetUser(user);
                if (result.isSuccessful)
                {
                    _user = result.data as UserProfile;
                    GenerateOTP();
                    log = new Log(2, "Info", username, "Authentication", "Data", "Successfull Logged In");
                    logger.Log(log);
                    result.isSuccessful = true;
                }
                else
                {
                    if (result.errorMessage.Equals("Error: Invalid Username or Password"))
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
            tcs.SetResult(result);
            return result;
        }

        public void GenerateOTP()
        {
            var random = new Random();
            int count = 0;
            while (count < 10)
            {
                int character = random.Next(3);
                // 0-9
                if (character == 0)
                {
                    _otp = _otp + random.Next(9).ToString();
                    count++;
                }
                // a-z
                if (character == 1)
                {
                    _otp = _otp + (char)random.Next(97, 123);
                    count++;
                }
                // A-Z
                if (character == 2)
                {
                    _otp = _otp + (char)random.Next(65, 91);
                    count++;
                }
            }
            _otpCreated = DateTime.Now;
        }

        public String SendOTP()
        {
            return _otp;
        }

        public Response VerifyOTP(String enteredOTP)
        {
            var result = new Response();
            if (_otp == null)
            {
                result.isSuccessful = false;
                result.errorMessage = "Error OTP not Generated";
                return result;
            }
            var currentTime = DateTime.Now;
            if (enteredOTP.Equals(_otp))
            {
                if ((currentTime.Ticks - _otpCreated.Ticks) < 1200000000) //12000000000 ticks in 2 minutes
                {
                    result.isSuccessful = true;
                    result.errorMessage = "OTP Verified";
                }
                else
                {
                    result.isSuccessful = false;
                    result.errorMessage = "OTP Expired, Please Authenticate Again";
                }
            }
            else
            {
                Console.WriteLine(_otp);
                result.isSuccessful = false;
                result.errorMessage = "Invalid OTP";
            }
            return result;
        }

        public Response LoginOTP(String enteredOTP)
        {
            var result = new Response();
            if (_otp == null)
            {
                result.isSuccessful = false;
                result.errorMessage = "Error OTP not Generated";
                return result;
            }
            var currentTime = DateTime.Now;
            if (enteredOTP.Equals(_otp))
            {
                if ((currentTime.Ticks - _otpCreated.Ticks) < 1200000000) //12000000000 ticks in 2 minutes
                {
                    result.isSuccessful = true;
                    result.errorMessage = "You have successfully logged in";
                    result.data = _user;
                    _otpVerified = true;
                }
                else
                {
                    result.isSuccessful = false;
                    result.errorMessage = "OTP Expired, Please Authenticate Again";
                }
            }
            else
            {
                result.isSuccessful = false;
                result.errorMessage = "Invalid username or password provided. Retry again or contact system administrator if issue persists";
            }
            return result;
        }

        public Response LogOut()
        {
            var response = new Response();
            if (_user != null)
            {
                _user = null;
                response.isSuccessful = true;
                response.errorMessage = "Logout successfully";
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

        public Response GetUserProfileTable(ref List<UserProfile> list, UserProfile userProfile)
        {
            var response = new Response();
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to data";
                return response;
            }
            var connection = @"Server=.;Database=TeamBigData.Utification.Users;Uid=root;Pwd=root;TrustServerCertificate=True;Encrypt=False";
            IDBSelecter selectDAO = new SqlDAO(connection);
            response = selectDAO.SelectUserProfileTable(ref list).Result;
            return response;
        }

        public Response GetUserAccountTable(ref List<UserAccount> list, UserProfile userProfile)
        {
            var response = new Response();
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to data";
                return response;
            }
            var connection = @"Server=.;Database=TeamBigData.Utification.Users;Uid=root;Pwd=root;TrustServerCertificate=True;Encrypt=False";
            IDBSelecter selectDAO = new SqlDAO(connection);
            response = selectDAO.SelectUserAccountTable(ref list).Result;
            return response;
        }
        public Response EnableAccount(String disabledUser, UserProfile userProfile)
        {
            var response = new Response();
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to data";
                return response;
            }
            var connectionString = @"Server=.;Database=TeamBigData.Utification.Users;Uid=root;Pwd=root;TrustServerCertificate=True;Encrypt=False";
            var userDao = new SqlDAO(connectionString);
            //Find What they want to reset password to
            var findTask = userDao.GetNewPassword(disabledUser).Result;
            //Change Password
            if(findTask.isSuccessful)
            {
                var changeTask = userDao.ResetAccount(disabledUser, (String)findTask.data).Result;
                if (changeTask.isSuccessful)
                {
                    //Mark Request as Fullfilled
                    var requestConnectionString = @"Server=.;Database=TeamBigData.Utification.Users;Uid=root;Pwd=root;TrustServerCertificate=True;Encrypt=False";
                    var requestDB = new SqlDAO(requestConnectionString);
                    var RequestFulfilled = requestDB.RequestFulfilled(disabledUser).Result;
                    response = RequestFulfilled;
                }
                else
                {
                    response = changeTask;
                }
            }
            else
            {
                response = findTask;
            }
            return response;
        }

        public async Task<Response> RecoverAccount(String username, Byte[] encryptedPassword, Encryptor encryptor, String enteredOTP)
        {
            Response result = new Response();
            result.isSuccessful = false;
            //decrypt password
            String newPassword = encryptor.decryptString(encryptedPassword);
            //check if its valid
            if(!AccountRegisterer.IsValidPassword(newPassword))
            {
                result.errorMessage = "Invalid new password. Please make it at least 8 characters and no weird symbols";
                return result;
            }
            Response otpResponse = VerifyOTP(enteredOTP);
            if (!otpResponse.isSuccessful)
            {
                result.errorMessage = "Invalid username or OTP provided. Retry again or contact system administrator";
                return result;
            }
            //if valid password and otp, then we can hash password and proceed
            var hasher = new SecureHasher();
            //TODO: Add Salt to hash
            var newDigest = SecureHasher.HashString(username, newPassword);
            var connectionString = @"Server=.;Database=TeamBigData.Utification.Users;Uid=root;Pwd=root;TrustServerCertificate=True;Encrypt=False";
            SqlDAO userDao = new SqlDAO(connectionString);
            result = await userDao.CreateRecoveryRequest(username, newDigest);
            return result;
        }

        public Response GetRecoveryRequests(ref List<string> requests, UserProfile userProfile)
        {
            var response = new Response();
            if(!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to data";
                return response;
            }
            var connectionString = @"Server=.;Database=TeamBigData.Utification.Users;Uid=root;Pwd=root;TrustServerCertificate=True;Encrypt=False";
            var recoveryDao = new SqlDAO(connectionString);
            response = recoveryDao.GetRecoveryRequests(ref requests).Result;
            response.isSuccessful = true;
            return response;
        }
    }
}