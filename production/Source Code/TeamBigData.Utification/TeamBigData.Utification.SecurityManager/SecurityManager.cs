
﻿using System.Collections;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text.RegularExpressions;
using TeamBigData.Utification.AccountServices;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.Manager.Abstractions;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
namespace TeamBigData.Utification.Manager
{
    public class SecurityManager : IRegister, ILogin, ILogout
    {
        
        // Does Insert user and doesnt need AccountRegisterer.
        // Does this need to be async?
        public async Task<Response> RegisterUser(string email, byte[] encryptedPassword, Encryptor encryptor)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response response = new Response();
            UserProfile userProfile = new UserProfile();
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            IDBInserter sqlUserIDAO = new SqlDAO(connectionString);
            IDBSelecter sqlUserSDAO = new SqlDAO(connectionString);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int userID = 0;
            String password = encryptor.decryptString(encryptedPassword);
            String salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(24));
            var digest = SecureHasher.HashString(salt, password);
            String pepper = "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI";
            var userHash = SecureHasher.HashString(pepper, email);
            var userAccount = new UserAccount(userID, email, digest, salt, userHash);
            response = await sqlUserIDAO.InsertUser(userAccount).ConfigureAwait(false);
            if (!response.isSuccessful)
            {
                if (response.errorMessage.Contains("Violation of UNIQUE KEY"))
                {
                    response.errorMessage = "Email already linked to an account, please pick a new email";
                    response.isSuccessful = false;
                    return response;
                }
                /*else if (response.errorMessage.Contains("Violation of UNIQUE KEY"))
                {
                    response.errorMessage = "Unable to assign username. Retry again or contact system administrator";
                }*/
            }
            else
            {
                var response2 = await sqlUserSDAO.SelectLastUserID().ConfigureAwait(false);
                userID = (int)response2.data;
                userProfile = new UserProfile(userID, "Regular User");
                response = await sqlUserIDAO.InsertUserProfile(userProfile).ConfigureAwait(false);
                stopwatch.Stop();
                Log log;
                var logger = new Logger(new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
                if (response.isSuccessful)
                {
                    IDBInserter insertUserHash = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.UserHash;Integrated Security=True;Encrypt=False");
                    await insertUserHash.InsertUserHash(userHash, userID).ConfigureAwait(false);
                    if (stopwatch.ElapsedMilliseconds > 5000)
                    {
                        log = new Log(1, "Warning", userHash, "SecurityManager.RegisterUser()", "Data", "Account Registration Took Longer Than 5 Seconds");
                    }
                    else
                    {
                        log = new Log(1, "Info", userHash, "SecurityManager.RegisterUser()", "Data", "Account Registration Succesful");
                    }
                }
                else
                {
                    log = new Log(1, "Error", userHash, "SecurityManager.RegisterUser()", "Data", "Error in Creating Account");
                }
                var responselog = logger.Log(log).Result;
                response.errorMessage = "Account created successfully, your username is " + email;
                response.isSuccessful = true;
            }
            return response;
        }
        public async Task<Response> RegisterUserAdmin(string email, byte[] encryptedPassword, Encryptor encryptor, UserProfile userProfileA)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response response = new Response();
            UserAccount userAccount = new UserAccount();
            UserProfile userProfile = new UserProfile();
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            IDBInserter sqlUserIDAO = new SqlDAO(connectionString);
            IDBSelecter sqlUserSDAO = new SqlDAO(connectionString);
            if (!((IPrincipal)userProfileA).IsInRole("Admin User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to view";
                return response;
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int userID = 0;
            String password = encryptor.decryptString(encryptedPassword);
            String salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(24));
            var digest = SecureHasher.HashString(salt, password);
            String pepper = "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI";
            var userHash = SecureHasher.HashString(pepper, email);
            response = await sqlUserSDAO.SelectLastUserID().ConfigureAwait(false);
            if ((int)response.data == 0)
            {
                userID = 1001;
                userAccount = new UserAccount(userID, email, digest, salt, userHash);
                response.data = "UserAccount Created";
            }
            else
            {
                userID = (int)response.data + 1;
                userAccount = new UserAccount(userID, email, digest, salt, userHash);
                response.data = "UserAccount Created";
            }
            response = await sqlUserIDAO.InsertUser(userAccount).ConfigureAwait(false);
            if (!response.isSuccessful)
            {
                if (response.errorMessage.Contains("Violation of UNIQUE KEY"))
                {
                    response.errorMessage = "Email already linked to an account, please pick a new email";
                    response.isSuccessful = false;
                    return response;
                }
                /*else if (response.errorMessage.Contains("Violation of UNIQUE KEY"))
                {
                    response.errorMessage = "Unable to assign username. Retry again or contact system administrator";
                }*/
            }
            else
            {
                userProfile = new UserProfile(userID, "Admin User");
                response = await sqlUserIDAO.InsertUserProfile(userProfile).ConfigureAwait(false);
                stopwatch.Stop();
                Log log;
                var logger = new Logger(new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
                if (response.isSuccessful)
                {
                    IDBInserter insertUserHash = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.UserHash;Integrated Security=True;Encrypt=False");
                    await insertUserHash.InsertUserHash(userHash, userID).ConfigureAwait(false);
                    if (stopwatch.ElapsedMilliseconds > 5000)
                    {
                        log = new Log(1, "Warning", userHash, "SecurityManager.RegisterUser()", "Data", "Account Registration Took Longer Than 5 Seconds");
                    }
                    else
                    {
                        log = new Log(1, "Info", userHash, "SecurityManager.RegisterUser()", "Data", "Account Registration Succesful");
                    }
                }
                else
                {
                    log = new Log(1, "Error", userHash, "SecurityManager.RegisterUser()", "Data", "Error in Creating Account");
                }
                var responselog = logger.Log(log).Result;
                response.errorMessage = "Account created successfully, your username is " + email;
                response.isSuccessful = true;
            }
            return response;
        }
        public Task<Response> LoginUser(String email, byte[] encryptedPassword, Encryptor encryptor, ref UserAccount userAccount, ref UserProfile userProfile)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response response = new Response();
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            IDBSelecter sqlUserSDAO = new SqlDAO(connectionString);
            IDBInserter sqlUserIDAO = new SqlDAO(connectionString);
            Log log;
            var logger = new Logger(new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            if (userProfile.Identity.AuthenticationType != "Anonymous User")
            {
                response.isSuccessful = false;
                response.errorMessage = "Error You are already Logged In";
                tcs.SetResult(response);
                return tcs.Task;
            }
            sqlUserSDAO.SelectUserAccount(ref userAccount, email);
            if (userAccount._verified == false)
            {
                response.isSuccessful = false;
                response.errorMessage = "Error: Account disabled. Perform account recovery or contact system admin";
                tcs.SetResult(response);
                return tcs.Task;
            }
            if (userAccount._userID == 0)
            {
                IDBInserter insertUserHash = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.UserHash;Integrated Security=True;Encrypt=False");
                String pepper = "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI";
                var userHash = SecureHasher.HashString(pepper, email);
                insertUserHash.InsertUserHash(userHash, 0);
                response.errorMessage = "User doesn't exist.";
                log = new Log(1, "Error", userHash, "SecurityManager.LoginUser()", "Data", "Error UserAccount doesn't exist.");
                tcs.SetResult(response);
                return tcs.Task;
            }
            string password = encryptor.decryptString(encryptedPassword);
            String digest = SecureHasher.HashString(userAccount._salt, password);
            if (userAccount._password != digest)
            {
                sqlUserIDAO.IncrementUserAccountDisabled(userAccount);
                response.isSuccessful = false;
                response.errorMessage = "Invalid username or password provided. Retry again or contact system administrator";
                tcs.SetResult(response);
                return tcs.Task;
            }
            sqlUserSDAO.SelectUserProfile(ref userProfile, userAccount._userID);
            if (userProfile._userID == 0)
            {
                IDBInserter insertUserHash = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.UserHash;Integrated Security=True;Encrypt=False");
                String pepper = "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI";
                var userHash = SecureHasher.HashString(pepper, email);
                insertUserHash.InsertUserHash(userHash, 0);
                response.errorMessage = "User doesn't exist.";
                log = new Log(1, "Error", userHash, "SecurityManager.LoginUser()", "Data", "Error UserProfile doesn't exist.");
                tcs.SetResult(response);
                return tcs.Task;
            }
            response.isSuccessful = true;
            response.errorMessage = "User is successfully authenticated.";
            tcs.SetResult(response);
            return tcs.Task;
        }
        public Task<Response> LogOutUser(ref UserAccount userAccount, ref UserProfile userProfile)
        {
            var tcs = new TaskCompletionSource<Response>();
            Log log;
            var logger = new Logger(new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            Response response = new Response();
            if (userAccount._userID != 0)
            {
                log = new Log(1, "Info", userAccount._userHash, "SecurityManager.LogOutUser()", "Data", "Logout successfully");
                userAccount = new UserAccount();
                userProfile= new UserProfile();
                response.isSuccessful = true;
                response.errorMessage = "Logout successfully";
            }
            else
            {
                log = new Log(1, "Error", userAccount._userHash, "SecurityManager.LogOutUser()", "Data", "Error you are not logged in");
                response.isSuccessful = false;
                response.errorMessage = "Error you are not logged in";
            }
            tcs.SetResult(response);
            return tcs.Task;
        }
        // not sure if working
        public Task<Response> EnableAccount(String disabledUser, UserProfile userProfile)
        {
            var tcs = new TaskCompletionSource<Response>();
            var response = new Response();
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to data";
                tcs.SetResult(response);
                return tcs.Task;
            }
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            var userDao = new SqlDAO(connectionString);
            var enabler = new AccountDisabler(userDao);
            var enableTask = enabler.EnableAccount(disabledUser).Result;
            response = enableTask;
            tcs.SetResult(response);
            return tcs.Task;
        }

        private UserProfile? _user;
        private String _otp;
        private DateTime _otpCreated;
        private Boolean _otpVerified;

        public SecurityManager()
        {
            _user = null;
        }

        public Response Register(ref UserAccount userAccount, ref UserProfile userProfile)
        {
        //Moved to View
            Response response = new Response();
            Console.WriteLine("To create a new Account, please enter your email");
            String email = Console.ReadLine();
            Console.WriteLine("Please enter your new password");
            String userPassword = Console.ReadLine();
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString(userPassword);
            var connection = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            IDBInserter insert = new SqlDAO(connection);
            AccountRegisterer accountRegisterer = new AccountRegisterer(insert);
            response = InsertUser(email, encryptedPassword, encryptor);
            return response;
        }
        public Response InsertUser(String email, byte[] encryptedPassword, Encryptor encryptor)
        {
            var response = new Response();
            Stopwatch stopwatch = new Stopwatch();
            String userHash = "";
            response.isSuccessful = false;
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            IDBInserter sqlIDAO = new SqlDAO(connectionString);
            IDBSelecter sqlSDAO = new SqlDAO(connectionString);
            var accountManager = new AccountRegisterer(sqlIDAO);
            stopwatch.Start();
            response = accountManager.InsertUser(email, encryptedPassword, encryptor).Result;
            stopwatch.Stop();
            Log log;
            var logger = new Logger(new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            if (response.isSuccessful)
            {
                String username = response.errorMessage.Substring(47);
                String pepper = "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI";
                userHash = SecureHasher.HashString(pepper, email);
                IDBInserter insertUserHash = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.UserHash;Integrated Security=True;Encrypt=False");
                insertUserHash.InsertUserHash(userHash, (int)sqlSDAO.SelectLastUserID().Result.data);
                if (stopwatch.ElapsedMilliseconds > 5000)
                {
                    log = new Log(1, "Warning", userHash, "Manager.InsertUser()", "Data", "Account Registration Took Longer Than 5 Seconds");
                }
                else
                {
                    log = new Log(1, "Info", userHash, "Manager.InsertUser()", "Data", "Account Registration Succesful");
                }
            }
            else
            {
                log = new Log(1, "Error", userHash, "Manager.InsertUser()", "Data", "Error in Creating Account");
            }
            logger.Log(log);
            var result2 = sqlIDAO.InsertUserProfile(new UserProfile((int)sqlSDAO.SelectLastUserID().Result.data, "Regular User")).Result;
            Console.WriteLine(result2.errorMessage);
            return response;
        }
        public Response InsertUserAdmin(String email, byte[] encryptedPassword, Encryptor encryptor,UserProfile userProfile)
        {
            var response = new Response();
            Stopwatch stopwatch = new Stopwatch();
            String userHash = "";
            response.isSuccessful = false;
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to view";
                return response;
            }
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            IDBInserter sqlIDAO = new SqlDAO(connectionString);
            IDBSelecter sqlSDAO = new SqlDAO(connectionString);
            var accountManager = new AccountRegisterer(sqlIDAO);
            stopwatch.Start();
            response = accountManager.InsertUser(email, encryptedPassword, encryptor).Result;
            stopwatch.Stop();
            Log log;
            var logger = new Logger(new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            if (response.isSuccessful)
            {
                String username = response.errorMessage.Substring(47);
                String pepper = "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI";
                userHash = SecureHasher.HashString(pepper, email);
                IDBInserter insertUserHash = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.UserHash;Integrated Security=True;Encrypt=False");
                insertUserHash.InsertUserHash(userHash, (int)sqlSDAO.SelectLastUserID().Result.data);
                if (stopwatch.ElapsedMilliseconds > 5000)
                {
                    log = new Log(1, "Warning", userHash, "Manager.InsertUser()", "Data", "Account Registration Took Longer Than 5 Seconds");
                }
                else
                {
                    log = new Log(1, "Info", userHash, "Manager.InsertUser()", "Data", "Account Registration Succesful");
                }
            }
            else
            {
                log = new Log(1, "Error", userHash, "Manager.InsertUser()", "Data", "Error in Creating Account");
            }
            logger.Log(log);
            var result2 = sqlIDAO.InsertUserProfile(new UserProfile((int)sqlSDAO.SelectLastUserID().Result.data, "Admin User")).Result;
            Console.WriteLine(result2.errorMessage);
            return response;
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
            var connection = @"Server=.\;Database=TeamBigData.Utification.UserProfile;Integrated Security=True;Encryption=False";
            IDBSelecter selectDAO = new SqlDAO(connection);
            response = selectDAO.SelectUserProfileTable(ref list, "Admin User").Result;
            return response;
        }

        public Response GetUserAccountTable(List<UserAccount> list, UserProfile userProfile)
        {
            var response = new Response();
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to data";
                return response;
            }
            var connection = @"Server=.\;Database=TeamBigData.Utification.UserProfile;Integrated Security=True;Encryption=False";
            IDBSelecter selectDAO = new SqlDAO(connection);
            response = selectDAO.SelectUserAccountTable(ref list, userProfile.Identity.AuthenticationType).Result;
            response.isSuccessful = true;
            return response;
        }

        public async Task<Response> VerifyUser(String username, byte[] encryptedPassword, Encryptor encryptor)
        {
            var tcs = new TaskCompletionSource<Response>();
            var result = new Response();
            if (true)//(_user == null)
            {
                var logConnectionString = @"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True";
                var logDAO = new SqlDAO(logConnectionString);
                ILogger logger = new Logger(logDAO);
                Log log;
                var password = encryptor.decryptString(encryptedPassword);
                if (!AccountRegisterer.IsValidPassword(password) || !AccountRegisterer.IsValidEmail(username))
                {
                    result.isSuccessful = false;
                    result.errorMessage = "Invalid username or password provided. Retry again or contact system administrator";
                    return result;
                }
                var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
                IDBSelecter selectDao = new SqlDAO(connectionString);
                UserAccount userAccount = new UserAccount();
                await selectDao.SelectUserAccount(ref userAccount, username);
                if (SecureHasher.HashString(userAccount._salt, password) == userAccount._password)
                {
                    result.isSuccessful = true;
                }
                if (result.isSuccessful)
                {
                    //_user = result.data as UserProfile;
                    log = new Log(2, "Info", SecureHasher.HashString("5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI", username), "Authentication", "Data", "Successfull Logged In");
                    logger.Log(log);
                    result.isSuccessful = true;
                }
                else
                {
                    if (result.errorMessage.Equals("Error: Invalid Username or Password"))
                    {
                        log = new Log(2, "Warning", SecureHasher.HashString(username, "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI"), "Authentication", "Data", "Insuccessful Log In Attempt");
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
                                //var disabler = new AccountDisabler(userDao);
                                //disabler.DisableAccount(username);
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

        /*public Response GetUserProfileTable(ref List<UserProfile> list, UserProfile userProfile)
        {
            var response = new Response();
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to data";
                return response;
            }
            var connection = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
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
            var connection = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            IDBSelecter selectDAO = new SqlDAO(connection);
            response = selectDAO.SelectUserAccountTable(ref list).Result;
            return response;
        }*/
        public Response ResetAccount(String disabledUser, UserProfile userProfile)
        {
            var response = new Response();
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to data";
                return response;
            }
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
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
                    var requestConnectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
                    var requestDB = new SqlDAO(requestConnectionString);
                    var RequestFulfilled = requestDB.RequestFulfilled(disabledUser).Result;
                    if(RequestFulfilled.isSuccessful)
                    {
                        response.isSuccessful = true;
                        response.errorMessage = "Account recovery completed successfully for user";
                    }
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
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            SqlDAO userDao = new SqlDAO(connectionString);
            UserAccount userAccount = new UserAccount();
            var selectResponse = userDao.SelectUserAccount(ref userAccount, username).Result;
            if(!selectResponse.isSuccessful)
            {
                result.errorMessage = "Invalid username or OTP provided. Retry again or contact system administrator";
                return result;
            }
            //if valid password and otp, then we can hash password and proceed
            var hasher = new SecureHasher();
            //TODO: Add Salt to hash
            var newDigest = SecureHasher.HashString(userAccount._salt, newPassword);
            result = await userDao.CreateRecoveryRequest(userAccount._userID, newDigest);
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
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security = True;Encrypt=False";
            var recoveryDao = new SqlDAO(connectionString);
            response = recoveryDao.GetRecoveryRequests(ref requests).Result;
            response.isSuccessful = true;
            return response;
        }
        public Response DisableAccount(String enabledUser, UserProfile userProfile)
        {
            var response = new Response();
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to data";
                return response;
            }
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            var userDao = new SqlDAO(connectionString);
            var disabler = new AccountDisabler(userDao);
            var disableTask = disabler.DisableAccount(enabledUser).Result;
            response = disableTask;
            return response;
        }

        public Response ChangePassword(String updateUser, UserProfile userProfile, Encryptor encryptor, Byte[] encryptedPass)
        {
            var response = new Response();
            
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to data";
                return response;
            }
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            var userDao = new SqlDAO(connectionString);
            //var updater = new AccountDisabler(userDao);
            var decrypted = encryptor.decryptString(encryptedPass);
            var hashPassword = SecureHasher.HashString(updateUser, decrypted);
            //TODO
            //      Add Salt
            response = userDao.ChangePassword(updateUser,hashPassword).Result;
            return response;
        }

        public Response DeleteProfile(String delUser, UserProfile userProfile)
        {
            var response = new Response();
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to data";
                return response;
            }
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            var userDao = new SqlDAO(connectionString);
            var userDelDao = new SQLDeletionDAO(connectionString);
            IDBSelecter testDBO = new SqlDAO(connectionString);
            //var updater = new AccountDisabler(userDao);
            UserAccount userAccount = new UserAccount();
            response = userDao.SelectUserAccount(ref userAccount, delUser).Result;
           
            var deleter = userDao.DeleteUserProfile(userAccount._userID).Result;
            response = deleter;
            return response;
        }
        // public async Task<bool> BulkFileUpload(IFormFile file)
        //{
        // var response = new Response();
        //    if (file.fileSize > 2147483648)
        //    {
        //         response.isSuccessful = false
        //         response.errorMessge = "";
        //     }
        // }
    }
}