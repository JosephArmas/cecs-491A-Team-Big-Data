using System.Collections;
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
        public Response RegisterUser(string email, byte[] encryptedPassword, Encryptor encryptor)
        {
            Response response = new Response();
            UserAccount userAccount = new UserAccount();
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
            response = sqlUserSDAO.SelectLastUserID();
            if ((int)response.data == 0)
            {
                userID = 1001;
                userAccount = new UserAccount(userID, email, digest, salt, userHash);
                response.data = "UserAccount Created";
            }
            else
            {
                userID = (int)sqlUserSDAO.SelectLastUserID().data + 1;
                userAccount = new UserAccount(userID, email, digest, salt, userHash);
                response.data = "UserAccount Created";
            }
            response = sqlUserIDAO.InsertUser(userAccount);
            if (!response.isSuccessful)
            {
                if (response.errorMessage.Contains("Violation of PRIMARY KEY"))
                {
                    response.errorMessage = "Email already linked to an account, please pick a new email";
                }
                else if (response.errorMessage.Contains("Violation of UNIQUE KEY"))
                {
                    response.errorMessage = "Unable to assign username. Retry again or contact system administrator";
                }
            }
            else
            {
                userProfile = new UserProfile(userID, "Regular User");
                response = sqlUserIDAO.InsertUserProfile(userProfile);
                stopwatch.Stop();
                Log log;
                var logger = new Logger(new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
                if (response.isSuccessful)
                {
                    IDBInserter insertUserHash = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.UserHash;Integrated Security=True;Encrypt=False");
                    insertUserHash.InsertUserHash(userHash, userID);
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
                Console.WriteLine(responselog.errorMessage);
                response.errorMessage = "Account created successfully, your username is " + email;
            }
            return response;
        }
        public Response LoginUser(String email, byte[] encryptedPassword, Encryptor encryptor, ref UserAccount userAccount, ref UserProfile userProfile)
        {
            Response response = new Response();
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            IDBSelecter sqlUserSDAO = new SqlDAO(connectionString);
            Log log;
            var logger = new Logger(new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            response = sqlUserSDAO.SelectUserAccount(email, ref userAccount);
            if (userAccount._userID == 0)
            {
                IDBInserter insertUserHash = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.UserHash;Integrated Security=True;Encrypt=False");
                String pepper = "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI";
                var userHash = SecureHasher.HashString(pepper, email);
                insertUserHash.InsertUserHash(userHash, 0);
                response.errorMessage = "User doesn't exist.";
                log = new Log(1, "Error", userHash, "SecurityManager.LoginUser()", "Data", "Error UserAccount doesn't exist.");
                return response;
            }
            response = sqlUserSDAO.SelectUserProfile(userAccount._userID, ref userProfile);
            if (userProfile._userID == 0)
            {
                IDBInserter insertUserHash = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.UserHash;Integrated Security=True;Encrypt=False");
                String pepper = "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI";
                var userHash = SecureHasher.HashString(pepper, email);
                insertUserHash.InsertUserHash(userHash, 0);
                response.errorMessage = "User doesn't exist.";
                log = new Log(1, "Error", userHash, "SecurityManager.LoginUser()", "Data", "Error UserProfile doesn't exist.");
                return response;
            }
            response.isSuccessful = true;
            response.errorMessage = "User is successfully authenticated.";
            return response;
        }
        public Response LogOutUser(ref UserAccount userAccount, ref UserProfile userProfile)
        {
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
            return response;
        }
        // not sure if working
        public Response EnableAccount(String disabledUser, UserProfile userProfile)
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
            var enabler = new AccountDisabler(userDao);
            var enableTask = enabler.EnableAccount(disabledUser).Result;
            response = enableTask;
            return response;
        }

        /*
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
                insertUserHash.InsertUserHash(userHash, (int)sqlSDAO.SelectLastUserID().data);
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
            var result2 = sqlIDAO.InsertUserProfile(new UserProfile((int)sqlSDAO.SelectLastUserID().data, "Regular User")).Result;
            Console.WriteLine(result2.errorMessage);
            return response;
        }
        
        public Response GetUserProfileTable(List<UserProfile> list, UserProfile userProfile)
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
            list = selectDAO.SelectUserProfileTable();
            response.isSuccessful = true;
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
            list = selectDAO.SelectUserAccountTable();
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
                UserAccount userAccount = selectDao.SelectUserAccount(username);
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
        */
    }
}