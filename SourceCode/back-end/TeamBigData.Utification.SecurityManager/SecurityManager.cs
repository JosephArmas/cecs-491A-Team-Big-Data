using TeamBigData.Utification.AccountServices;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Manager.Abstractions;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.DTO;

namespace TeamBigData.Utification.Manager
{
    public class SecurityManager : IRegister, ILogin
    {
        private readonly AccountRegisterer _accountRegisterer;
        private readonly UserhashServices _userhashServices;
        private readonly AccountAuthentication _accountAuthentication;

        public SecurityManager(AccountRegisterer accountRegisterer, UserhashServices userhashServices, AccountAuthentication accountAuthentication)
        {
            _accountRegisterer = accountRegisterer;
            _userhashServices = userhashServices;
            _accountAuthentication = accountAuthentication;
        }

        // Does Insert user and doesnt need AccountRegisterer.
        public async Task<Response> RegisterUser(String email, String password)
        {
            // TODO: Check if process time follows business rules

            Response result = new Response();

            String pepper = "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI";
            var userHash = SecureHasher.HashString(email, pepper);

            var data = await _accountRegisterer.InsertUserAccount(email, password, userHash).ConfigureAwait(false);
            if (!data.isSuccessful)
            {
                result.errorMessage = data.errorMessage + ", {failed: _accountRegisterer.InsertUser}";
                result.isSuccessful = false;
                return result;
            }

            result = await _accountRegisterer.InsertUserProfile(data.data).ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                result.errorMessage += ", {failed: _accountRegisterer.InsertUserProfile}";
                result.isSuccessful = false;
                return result;
            }

            result = await _userhashServices.InsertUserhash(userHash, data.data).ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                result.errorMessage += ", {failed: _accountRegisterer.InsertUserhash}";
                result.isSuccessful = false;
                return result;
            }
            else
            {
                result.isSuccessful = true;
                return result;
            }
        }
        


        public async Task<DataResponse<AuthenticateUserResponse>> LoginUser(String email, String password)
        {
            // TODO: Check if process time follows business rules

            var authenticateUserResponse = new DataResponse<AuthenticateUserResponse>();

            var userAccount = await _accountAuthentication.AuthenticateUserAccount(email, password).ConfigureAwait(false);
            if (!userAccount.isSuccessful)
            {
                authenticateUserResponse.isSuccessful = false;
                authenticateUserResponse.errorMessage = userAccount.errorMessage + ", {failed: _accountAuthentication.AuthenticateUserAccount}";
                return authenticateUserResponse;
            }

            var userProfile = await _accountAuthentication.AuthenticatedUserProfile(userAccount.data._userID).ConfigureAwait(false);
            if (!userProfile.isSuccessful)
            {
                authenticateUserResponse.isSuccessful = false;
                authenticateUserResponse.errorMessage = userProfile.errorMessage + ", {failed: _accountAuthentication.AuthenticatedUserProfile}";
                return authenticateUserResponse;
            }
            else
            {
                userAccount.data.GenerateOTP();
                authenticateUserResponse.data = new AuthenticateUserResponse(userAccount.data._userID, userAccount.data._username, userAccount.data._otp, userAccount.data._otpCreated, userProfile.data.Identity.AuthenticationType, userAccount.data._userHash);
                authenticateUserResponse.isSuccessful = true;
            }

            return authenticateUserResponse;
        }

        /*
         * 
         * 
         *public async Task<Response> RegisterUserAdmin(string email, byte[] encryptedPassword, Encryptor encryptor, UserProfile userProfileA)
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
            response = await sqlUserIDAO.InsertUser("","","","").ConfigureAwait(false);
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
                }
            }
            else
            {
                userProfile = new UserProfile(userID, "Admin User");
                response = await sqlUserIDAO.InsertUserProfile(0).ConfigureAwait(false);
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
                var responselog = await logger.Log(log);
                response.errorMessage = "Account created successfully, your username is " + email;
                response.isSuccessful = true;
            }
            return response;
        }
         * Not sure if needed
        public async Task<DataResponse<UserProfile>> LogOutUser(UserProfile userProfile, String userhash)
        {
            Log log;
            var logger = new Logger(new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            var response = new DataResponse<UserProfile>();
            var newProfile = new UserProfile();
            if (!((IPrincipal)userProfile).IsInRole("Anonymous User"))
            {
                log = new Log(1, "Info", userhash, "SecurityManager.LogOutUser()", "Data", "Logout successfully");
                response.isSuccessful = true;
                response.data = newProfile;
                response.errorMessage = "Logout successfully";
            }
            else
            {
                response.isSuccessful = false;
                response.errorMessage = "Error you are not logged in";
            }
            return response;
        }
        // not sure if working
        public async Task<Response> EnableAccount(String disabledUser, UserProfile userProfile)
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
            var enableTask = await enabler.EnableAccount(disabledUser);
            response = enableTask;
            return response;
        }

        private UserProfile? _user;
        private String _otp;
        private DateTime _otpCreated;
        private Boolean _otpVerified;

        public SecurityManager()
        {
            _user = null;
        }

        /*public async Task<Response> InsertUser(String email, byte[] encryptedPassword, Encryptor encryptor)
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
            response = await accountManager.InsertUser(email, encryptedPassword, encryptor);
            stopwatch.Stop();
            Log log;
            var logger = new Logger(new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            if (response.isSuccessful)
            {
                String username = response.errorMessage.Substring(47);
                String pepper = "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI";
                userHash = SecureHasher.HashString(pepper, email);
                IDBInserter insertUserHash = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.UserHash;Integrated Security=True;Encrypt=False");
                var userIdResponse = await sqlSDAO.SelectLastUserID();
                insertUserHash.InsertUserHash(userHash, (int)userIdResponse.data);
                sqlIDAO.InsertUserProfile(new UserProfile((int)userIdResponse.data, "Regular User"));
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
            return response;
        }
        public async Task<Response> InsertUserAdmin(String email, byte[] encryptedPassword, Encryptor encryptor,UserProfile userProfile)
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
            response = await accountManager.InsertUser(email, encryptedPassword, encryptor);
            stopwatch.Stop();
            Log log;
            var logger = new Logger(new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            if (response.isSuccessful)
            {
                String username = response.errorMessage.Substring(47);
                String pepper = "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI";
                userHash = SecureHasher.HashString(pepper, email);
                IDBInserter insertUserHash = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.UserHash;Integrated Security=True;Encrypt=False");
                var idResponse = await sqlSDAO.SelectLastUserID();
                insertUserHash.InsertUserHash(userHash, (int)idResponse.data);
                sqlIDAO.InsertUserProfile(new UserProfile((int)idResponse.data, "Admin User"));
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
            return response;
        }

        public async Task<DataResponse<List<UserProfile>>> GetUserProfileTable(UserProfile userProfile)
        {
            var response = new DataResponse<List<UserProfile>>();
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to data";
                return response;
            }
            var connection = @"Server=.\;Database=TeamBigData.Utification.UserProfile;Integrated Security=True;Encryption=False";
            IDBSelecter selectDAO = new SqlDAO(connection);
            response = await selectDAO.SelectUserProfileTable("Admin User");
            return response;
        }

        public async Task<DataResponse<List<UserAccount>>> GetUserAccountTable(UserProfile userProfile)
        {
            var response = new DataResponse<List<UserAccount>>();
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to data";
                return response;
            }
            var connection = @"Server=.\;Database=TeamBigData.Utification.UserProfile;Integrated Security=True;Encryption=False";
            IDBSelecter selectDAO = new SqlDAO(connection);
            response = await selectDAO.SelectUserAccountTable(userProfile.Identity.AuthenticationType);
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
                var accountResponse = await selectDao.SelectUserAccount(username);
                var userAccount = accountResponse.data;
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
                        var attemptsResult = await logDAO.GetLoginAttempts(username);
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
                if ((currentTime.Ticks - _otpCreated.Ticks) < 1200000000) //1200000000 ticks in 2 minutes
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


        public async Task<Response> ResetAccount(int disabledUserId, UserProfile userProfile)
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
            var findTask = await userDao.GetNewPassword(disabledUserId);
            //Change Password
            if(findTask.isSuccessful)
            {
                var changeTask = await userDao.ResetAccount(disabledUserId, (String)findTask.data);
                if (changeTask.isSuccessful)
                {
                    //Mark Request as Fullfilled
                    var requestConnectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
                    var requestDB = new SqlDAO(requestConnectionString);
                    var RequestFulfilled = await requestDB.RequestFulfilled(disabledUserId);
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

        public async Task<Response> RecoverAccount(String username, Byte[] encryptedPassword, Encryptor encryptor)
        {
            var result = new Response();
            //decrypt password
            String newPassword = encryptor.decryptString(encryptedPassword);
            //check if its valid
            if(!AccountRegisterer.IsValidPassword(newPassword))
            {
                result.errorMessage = "Invalid new password. Please make it at least 8 characters and no weird symbols";
                return result;
            }
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            SqlDAO userDao = new SqlDAO(connectionString);
            var selectResponse = await userDao.SelectUserAccount(username);
            var userAccount = selectResponse.data;
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
            if(result.isSuccessful)
            {
                result.errorMessage = "Account recovery request sent";
            }
            return result;
        }

        public async Task<DataResponse<List<UserProfile>>> GetRecoveryRequests(UserProfile userProfile)
        {
            var response = new DataResponse<List<UserProfile>>();
            if(!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.isSuccessful = false;
                response.errorMessage = "Unauthorized access to data";
                return response;
            }
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security = True;Encrypt=False";
            var recoveryDao = new SqlDAO(connectionString);
            response = await recoveryDao.GetRecoveryRequests();
            return response;
        }

        public async Task<Response> DisableAccount(String enabledUser, UserProfile userProfile)
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
            var disableTask = await disabler.DisableAccount(enabledUser);
            response = disableTask;
            return response;
        }

        public async Task<Response> ChangePassword(String updateUser, UserProfile userProfile, Encryptor encryptor, Byte[] encryptedPass)
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
            response = await userDao.ChangePassword(updateUser, hashPassword);
            return response;
        }

        public async Task<Response> DeleteProfile(String delUser, UserProfile userProfile)
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
            var accountResponse = await userDao.SelectUserAccount(delUser);
            var userAccount = accountResponse.data;
            var deleter = await userDao.DeleteUserProfile(userAccount._userID);
            response = deleter;
            return response;
        }*/
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