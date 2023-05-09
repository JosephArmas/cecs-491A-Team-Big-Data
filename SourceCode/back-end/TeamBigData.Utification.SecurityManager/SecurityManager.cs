using Microsoft.Extensions.Logging;
using System.Security.Principal;
using TeamBigData.Utification.AccountServices;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Manager.Abstractions;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.LogsDB;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.SQLDataAccess.DTO;
using ILogger = TeamBigData.Utification.Logging.Abstraction.ILogger;
using TeamBigData.Utification.DeletionService;
using System.Diagnostics;
using System.Collections;
using System.Security.Cryptography;

namespace TeamBigData.Utification.Manager
{
    public class SecurityManager : IRegister, ILogin
    {
        private readonly AccountRegisterer? _accountRegisterer;
        private readonly UserhashServices? _userhashServices;
        private readonly AccountAuthentication? _accountAuthentication;
        private readonly RecoveryServices? _recoveryServices;
        private readonly ILogger? _logger;
        private readonly AccDeletionService? _accDeletionService;

        public SecurityManager(AccountRegisterer accountRegisterer, UserhashServices userhashServices, AccountAuthentication accountAuthentication, RecoveryServices recoveryServices, ILogger logger, AccDeletionService accDeletionService)
        {
            _accountRegisterer = accountRegisterer;
            _userhashServices = userhashServices;
            _accountAuthentication = accountAuthentication;
            _recoveryServices = recoveryServices;
            _logger = logger;
            _accDeletionService = accDeletionService;
        }


        //------------------------------------------------------------------------
        // AccountController
        //------------------------------------------------------------------------

        // Does Insert user and doesnt need AccountRegisterer.
        public async Task<Response> RegisterUser(String email, String password, String userhash)
        {
            // TODO: Check if process time follows business rules

            Response response = new Response();

            await _logger.Logs(new Log(0, "Info", userhash, "Register User Attempt", "Data", "User is attempting to register."));

            var userID = await _accountRegisterer.InsertUserAccount(email, password, userhash).ConfigureAwait(false);
            if (!userID.IsSuccessful)
            {
                await _logger.Logs(new Log(0, "Error", userhash, "_accountRegisterer.InsertUserAccount", "Data", "Failed to insert user account."));

                response.ErrorMessage = userID.ErrorMessage + ", {failed: _accountRegisterer.InsertUser}";
                response.IsSuccessful = false;
                return response;
            }

            response = await _accountRegisterer.InsertUserProfile(userID.Data).ConfigureAwait(false);
            if (!response.IsSuccessful)
            {
                await _logger.Logs(new Log(0, "Error", userhash, "_accountRegisterer.InsertUserProfile", "Data", "Failed to insert user profile."));

                response.ErrorMessage += ", {failed: _accountRegisterer.InsertUserProfile}";
                response.IsSuccessful = false;
                return response;
            }

            response = await _userhashServices.InsertUserhash(userhash, userID.Data).ConfigureAwait(false);
            if (!response.IsSuccessful)
            {
                await _logger.Logs(new Log(0, "Error", userhash, "_userhashServices.InsertUserhash", "Data", "Failed to insert userhash."));

                response.ErrorMessage += ", {failed: _accountRegisterer.InsertUserhash}";
                response.IsSuccessful = false;
                return response;
            }
            else
            {
                await _logger.Logs(new Log(0, "Info", userhash, "Passed Register User Attempt", "Data", "User successfully created an account."));

                response.IsSuccessful = true;
                return response;
            }
        }
        


        public async Task<DataResponse<AuthenticateUserResponse>> LoginUser(String email, String password, String userhash)
        {
            // TODO: Check if process time follows business rules

            var authenticateUserResponse = new DataResponse<AuthenticateUserResponse>();

            await _logger.Logs(new Log(0, "Info", userhash, "Login User Attempt", "Data", "User is attempting to login."));

            var userAccount = await _accountAuthentication.AuthenticateUserAccount(email, password).ConfigureAwait(false);
            if (!userAccount.IsSuccessful)
            {
                await _logger.Logs(new Log(0, "Error", userhash, "_accountAuthentication.AuthenticateUserAccount", "Data", "Failed to authenticate user account."));

                authenticateUserResponse.IsSuccessful = false;
                authenticateUserResponse.ErrorMessage = userAccount.ErrorMessage + ", {failed: _accountAuthentication.AuthenticateUserAccount}";
                return authenticateUserResponse;
            }

            var userProfile = await _accountAuthentication.AuthenticatedUserProfile(userAccount.Data.UserID).ConfigureAwait(false);
            if (!userProfile.IsSuccessful)
            {
                await _logger.Logs(new Log(0, "Error", userhash, "_accountAuthentication.AuthenticatedUserProfile", "Data", "Failed to insert user profile."));

                authenticateUserResponse.IsSuccessful = false;
                authenticateUserResponse.ErrorMessage = userProfile.ErrorMessage + ", {failed: _accountAuthentication.AuthenticatedUserProfile}";
                return authenticateUserResponse;
            }
            else
            {
                await _logger.Logs(new Log(0, "Info", userhash, "Passed Login User Attempt", "Data", "User is attempting to login."));

                userAccount.Data.GenerateOTP();
                authenticateUserResponse.Data = new AuthenticateUserResponse(userAccount.Data.UserID, userAccount.Data.Username, userAccount.Data.Otp, userAccount.Data.OtpCreated, userProfile.Data.Identity.AuthenticationType, userAccount.Data.UserHash);
                authenticateUserResponse.IsSuccessful = true;
            }

            return authenticateUserResponse;
        }

        public async Task<Response> DeleteUser(String email)
        {
            var userhash = SecureHasher.HashString("5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI", email);

            await _logger.Logs(new Log(0, "Info", userhash, "Delete User Attempt", "Data", "User is attempting to delete."));

            // Delete users PII
            var response = await _accDeletionService.DeletePII(email).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                await _logger.Logs(new Log(0, "Error", userhash, "_accDeletionService.DeletePII", "Data", "Failed to delete Users PII."));

                response.IsSuccessful = false;
                response.ErrorMessage += ", {failed: _accDeletionService.DeletePII}";
                return response;
            }

            await _logger.Logs(new Log(0, "Info", userhash, "Passed Delete User Attempt", "Data", "User is deleted."));

            response.IsSuccessful = true;
            return response;
        }


        //------------------------------------------------------------------------
        // RecoveryController
        //------------------------------------------------------------------------

        public async Task<Response> RecoverAccountPassword(String username, String password)
        {
            await _logger.Logs(new Log(0, "Info", username, "Recover Account Password Attempt", "Data", "User is attempting to login."));
            
            var response = await _recoveryServices.RequestRecoveryNewPassword(username, password);
            if (!response.IsSuccessful)
            {
                await _logger.Logs(new Log(0, "Error", username, "_recoveryServices.RequestRecoveryNewPassword", "Data", "User failed to login."));

                return new Response(false, response.ErrorMessage + ", {failed: _recoveryServices.RequestRecoveryNewPassword}");
            }
            else
            {
                await _logger.Logs(new Log(0, "Info", username, "Passed Recover Account Password Attempt", "Data", "User is attempting to login."));

                return new Response(true, response.ErrorMessage);
            }

        }

        public async Task<DataResponse<List<RecoveryRequests>>> GetRecoveryRequests(int userID)
        {
            await _logger.Logs(new Log(0, "Info", userID.ToString(), "Get Recovery Requests Attempt", "Data", "Admin is attempting to get recovery requests."));

            var dataResponse = await _recoveryServices.GetRecoveryRequestsTable().ConfigureAwait(false);
            if (!dataResponse.IsSuccessful)
            {
                await _logger.Logs(new Log(0, "Error", userID.ToString(), "_recoveryServices.GetRecoveryRequestsTable", "Data", "Admin failed to get recovery requests."));

                dataResponse.IsSuccessful = false;
                dataResponse.ErrorMessage += ", {failed: _recoveryServices.GetRecoveryRequestsTable}";
                return dataResponse;
            }
            else
            {
                await _logger.Logs(new Log(0, "Info", userID.ToString(), "Passed Get Recovery Requests Attempt", "Data", "User is attempting to login."));

                dataResponse.IsSuccessful = true;
            }

            return dataResponse;
        }

        public async Task<Response> ResetAccount(string disabledUsername, int adminID)
        {
            await _logger.Logs(new Log(0, "Info", adminID.ToString(), "Reset Account Attempt", "Data", "Admin is attempting to reset account."));
            
            var disabledUserId = (await _recoveryServices.GetUserIDbyName(disabledUsername)).Data;
            //Find What they want to reset password to
            var validRecovery = await _recoveryServices.GetNewPassword(disabledUserId).ConfigureAwait(false);
            if (!validRecovery.IsSuccessful)
            {
                await _logger.Logs(new Log(0, "Error", adminID.ToString(), "_recoveryServices.GetNewPassword", "Data", "Admin failed to get new password."));

                return new Response(false, validRecovery.ErrorMessage + ", {failed: _recoveryServices.GetNewPassword}");
            }

            //Change Password
            var response = await _recoveryServices.SaveNewPassword(disabledUserId, validRecovery.Data.Password, validRecovery.Data.Salt).ConfigureAwait(false);
            if (!response.IsSuccessful)
            {
                await _logger.Logs(new Log(0, "Error", adminID.ToString(), "_recoveryServices.SaveNewPassword", "Data", "Admin failed to save new password."));

                response.IsSuccessful = false;
                response.ErrorMessage += ", {failed: _recoveryServices.SaveNewPassword}";
                return response;
            }
            else
            {
                response.IsSuccessful = true;
            }

            return response;
            
        }
        /*public async Task<Response> RegisterUserAdmin(string email, byte[] encryptedPassword, Encryptor encryptor, UserProfile userProfileA)
        {
            var tcs = new TaskCompletionSource<Response>();
            Response response = new Response();
            UserAccount userAccount = new UserAccount();
            UserProfile userProfile = new UserProfile();
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            var sqlUserIDAO = new SqlDAO(connectionString);
            var sqlUserSDAO = new SqlDAO(connectionString);
            if (!((IPrincipal)userProfileA).IsInRole("Admin User"))
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Unauthorized access to view";
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
            if ((int)response.Data == 0)
            {
                userID = 1001;
                userAccount = new UserAccount(userID, email, digest, salt, userHash);
                response.Data = "UserAccount Created";
            }
            else
            {
                userID = (int)response.Data + 1;
                userAccount = new UserAccount(userID, email, digest, salt, userHash);
                response.Data = "UserAccount Created";
            }
            response = await sqlUserIDAO.InsertUser("","","","").ConfigureAwait(false);
            if (!response.IsSuccessful)
            {
                if (response.ErrorMessage.Contains("Violation of UNIQUE KEY"))
                {
                    response.ErrorMessage = "Email already linked to an account, please pick a new email";
                    response.IsSuccessful = false;
                    return response;
                }
                else if (response.ErrorMessage.Contains("Violation of UNIQUE KEY"))
                {
                    response.ErrorMessage = "Unable to assign username. Retry again or contact system administrator";
                }
            }
            else
            {
                userProfile = new UserProfile(userID, "Admin User");
                response = await sqlUserIDAO.InsertUserProfile(0).ConfigureAwait(false);
                stopwatch.Stop();
                Log log;
                var logger = new Logger(new LogsSqlDAO(@"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
                if (response.IsSuccessful)
                {
                    var insertUserHash = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.UserHash;Integrated Security=True;Encrypt=False");
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
                var responselog = await logger.Logs(log);
                response.ErrorMessage = "Account created successfully, your username is " + email;
                response.IsSuccessful = true;
            }
            return response;
        }
        public async Task<DataResponse<UserProfile>> LogOutUser(UserProfile userProfile, String userhash)
        {
            Log log;
            var logger = new Logger(new LogsSqlDAO(@"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            var response = new DataResponse<UserProfile>();
            var newProfile = new UserProfile();
            if (!((IPrincipal)userProfile).IsInRole("Anonymous User"))
            {
                log = new Log(1, "Info", userhash, "SecurityManager.LogOutUser()", "Data", "Logout successfully");
                response.IsSuccessful = true;
                response.Data = newProfile;
                response.ErrorMessage = "Logout successfully";
            }
            else
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Error you are not logged in";
            }
            return response;
        }

        private UserProfile? _user;
        private String _otp;
        private DateTime _otpCreated;
        private Boolean _otpVerified;

        public SecurityManager()
        {
        }


        public async Task<DataResponse<List<UserProfile>>> GetUserProfileTable(UserProfile userProfile)
        {
            var response = new DataResponse<List<UserProfile>>();
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Unauthorized access to data";
                return response;
            }
            var connection = @"Server=.\;Database=TeamBigData.Utification.UserProfile;Integrated Security=True;Encryption=False";
            var selectDAO = new SqlDAO(connection);
            response = await selectDAO.SelectUserProfileTable("Admin User");
            return response;
        }

        public async Task<DataResponse<List<UserAccount>>> GetUserAccountTable(UserProfile userProfile)
        {
            var response = new DataResponse<List<UserAccount>>();
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Unauthorized access to data";
                return response;
            }
            var connection = @"Server=.\;Database=TeamBigData.Utification.UserProfile;Integrated Security=True;Encryption=False";
            var selectDAO = new SqlDAO(connection);
            response = await selectDAO.SelectUserAccountTable(userProfile.Identity.AuthenticationType);
            response.IsSuccessful = true;
            return response;
        }

        public async Task<Response> VerifyUser(String username, byte[] encryptedPassword, Encryptor encryptor)
        {
            var tcs = new TaskCompletionSource<Response>();
            var result = new Response();
            if (true)//(_user == null)
            {
                var logConnectionString = @"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True";
                var logDAO = new LogsSqlDAO(logConnectionString);
                var dao = new SqlDAO(logConnectionString);
                ILogger logger = new Logger(logDAO);
                Log log;
                var password = encryptor.decryptString(encryptedPassword);
                if (!InputValidation.IsValidPassword(password) || !InputValidation.IsValidEmail(username))
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "Invalid username or password provided. Retry again or contact system administrator";
                    return result;
                }
                var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
                var selectDao = new SqlDAO(connectionString);
                var accountResponse = await selectDao.SelectUserAccount(username);
                var userAccount = accountResponse.Data;
                if (SecureHasher.HashString(userAccount.Salt, password) == userAccount.Password)
                {
                    result.IsSuccessful = true;
                }
                if (result.IsSuccessful)
                {
                    //_user = result.data as UserProfile;
                    log = new Log(2, "Info", SecureHasher.HashString("5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI", username), "Authentication", "Data", "Successfull Logged In");
                    logger.Logs(log);
                    result.IsSuccessful = true;
                }
                else
                {
                    if (result.ErrorMessage.Equals("Error: Invalid Username or Password"))
                    {
                        log = new Log(2, "Warning", SecureHasher.HashString(username, "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI"), "Authentication", "Data", "Insuccessful Log In Attempt");
                        logger.Logs(log);
                        result.IsSuccessful = false;
                        result.ErrorMessage = "Invalid username or password provided. Retry again or contact system administrator";
                        var attemptsResult = await dao.GetLoginAttempts(username);
                        var attempts = attemptsResult.Data as ArrayList;
                        if (attemptsResult.IsSuccessful)
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
                                result.ErrorMessage = "You have failed to login 3 times in 24 hours, your account will now be disabled";
                            }
                        }
                    }
                }
            }
            else
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Error You are already Logged In";
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
                result.IsSuccessful = false;
                result.ErrorMessage = "Error OTP not Generated";
                return result;
            }
            var currentTime = DateTime.Now;
            if (enteredOTP.Equals(_otp))
            {
                if ((currentTime.Ticks - _otpCreated.Ticks) < 1200000000) //1200000000 ticks in 2 minutes
                {
                    result.IsSuccessful = true;
                    result.ErrorMessage = "OTP Verified";
                }
                else
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "OTP Expired, Please Authenticate Again";
                }
            }
            else
            {
                Console.WriteLine(_otp);
                result.IsSuccessful = false;
                result.ErrorMessage = "Invalid OTP";
            }
            return result;
        }

        public Response LoginOTP(String enteredOTP)
        {
            var result = new Response();
            if (_otp == null)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Error OTP not Generated";
                return result;
            }
            var currentTime = DateTime.Now;
            if (enteredOTP.Equals(_otp))
            {
                if ((currentTime.Ticks - _otpCreated.Ticks) < 1200000000) //12000000000 ticks in 2 minutes
                {
                    result.IsSuccessful = true;
                    result.ErrorMessage = "You have successfully logged in";
                    result.Data = _user;
                    _otpVerified = true;
                }
                else
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "OTP Expired, Please Authenticate Again";
                }
            }
            else
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Invalid username or password provided. Retry again or contact system administrator if issue persists";
            }
            return result;
        }

        public Response LogOut()
        {
            var response = new Response();
            if (_user != null)
            {
                _user = null;
                response.IsSuccessful = true;
                response.ErrorMessage = "Logout successfully";
            }
            else
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Error you are not logged in";
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
                response.IsSuccessful = false;
                response.ErrorMessage = "Unauthorized access to data";
                return response;
            }
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            var userDao = new SqlDAO(connectionString);
            //Find What they want to reset password to
            var findTask = await userDao.GetNewPassword(disabledUserId);
            //Change Password
            if(findTask.IsSuccessful)
            {
                var changeTask = await userDao.ResetAccount(disabledUserId, (String)findTask.Data);
                if (changeTask.IsSuccessful)
                {
                    //Mark Request as Fullfilled
                    var requestConnectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
                    var requestDB = new SqlDAO(requestConnectionString);
                    var RequestFulfilled = await requestDB.RequestFulfilled(disabledUserId);
                    if(RequestFulfilled.IsSuccessful)
                    {
                        response.IsSuccessful = true;
                        response.ErrorMessage = "Account recovery completed successfully for user";
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
            if(!InputValidation.IsValidPassword(newPassword))
            {
                result.ErrorMessage = "Invalid new password. Please make it at least 8 characters and no weird symbols";
                return result;
            }
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            SqlDAO userDao = new SqlDAO(connectionString);
            var selectResponse = await userDao.SelectUserAccount(username);
            var userAccount = selectResponse.Data;
            if(!selectResponse.IsSuccessful)
            {
                result.ErrorMessage = "Invalid username or OTP provided. Retry again or contact system administrator";
                return result;
            }
            //if valid password and otp, then we can hash password and proceed
            var hasher = new SecureHasher();
            //TODO: Add Salt to hash
            var newDigest = SecureHasher.HashString(userAccount.Salt, newPassword);
            result = await userDao.CreateRecoveryRequest(userAccount.UserID, newDigest);
            if(result.IsSuccessful)
            {
                result.ErrorMessage = "Account recovery request sent";
            }
            return result;
        }

        public async Task<DataResponse<List<UserProfile>>> GetRecoveryRequests(UserProfile userProfile)
        {
            var response = new DataResponse<List<UserProfile>>();
            if(!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Unauthorized access to data";
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
                response.IsSuccessful = false;
                response.ErrorMessage = "Unauthorized access to data";
                return response;
            }
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            var userDao = new SqlDAO(connectionString);
            var disabler = new AccountDisabler(userDao);
            var disableTask = await disabler.DisableAccount(enabledUser);
            response = disableTask;
            return response;
        }

        public async Task<Response> EnableAccount(String enabledUser, UserProfile userProfile)
        {
            var response = new Response();
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Unauthorized access to data";
                return response;
            }
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            var userDao = new SqlDAO(connectionString);
            var disabler = new AccountDisabler(userDao);
            var disableTask = await disabler.EnableAccount(enabledUser);
            response = disableTask;
            return response;
        }

        public async Task<Response> ChangePassword(String updateUser, UserProfile userProfile, Encryptor encryptor, Byte[] encryptedPass)
        {
            var response = new Response();
            
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Unauthorized access to data";
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
        }*/

        /*
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
            var testDBO = new SqlDAO(connectionString);
            var accountResponse = await userDao.SelectUserAccount(delUser);
            var userAccount = accountResponse.data;
            var deleter = await userDao.DeleteUserProfile(userAccount._userID);
            response = deleter;
            return response;
        }
        */
        // public async Task<bool> BulkFileUpload(IFormFile file)
        //{
        // var response = new Response();
        //    if (file.fileSize > 2147483648)
        //    {
        //         response.isSuccessful = false
        //         response.errorMessge = "";
        //     }
        // }*/
    }
}