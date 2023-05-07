using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using TeamBigData.Utification.AccountServices;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Manager.Abstractions;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.UsersDB;
using TeamBigData.Utification.SQLDataAccess.UserhashDB;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.SQLDataAccess.LogsDB;

namespace TeamBigData.Utification.AccountDeletionTests
{
    [TestClass]
    public class AccountDeletionIntegrationTest
    {
        private readonly String featureString = @"Server=.\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False";
        private readonly String usersString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
        private readonly String logString = "Server=.\\;Database=TeamBigData.Utification.Logs;User=AppUser; Password=t; TrustServerCertificate=True; Encrypt=True";
        private readonly String hashString = "Server=.\\;Database=TeamBigData.Utification.UserHash;Integrated Security=True;Encrypt=False";

        /*[TestMethod]
        public async Task CanDeleteOwnAccountAsync()
        {
            //Arrange
            IDBSelecter testDBO = new SqlDAO(usersString);
            IDAO featDBO = new SqlDAO(featureString);
            // Manual Dependencies
            var userDAO = new UsersSqlDAO(usersString);
            var featureDAO = new SqlDAO(featureString);
            var hashDAO = new UserhashSqlDAO(hashString);
            var logDAO = new LogsSqlDAO(logString);
            var reg = new AccountRegisterer(userDAO, userDAO);
            var hash = new UserhashServices(hashDAO);
            var auth = new AccountAuthentication(userDAO);
            var rec = new RecoveryServices(userDAO, userDAO, userDAO);
            ILogger logger = new  Logger(new LogsSqlDAO(logString));
            var register = new SecurityManager(reg, hash, auth, rec, logger);


            var username = "Deletius" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(4)) + "@yahoo.com";
            var user = new UserProfile();
            var delMan = new DeletionManager();
            var userhash = SecureHasher.HashString(username, "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI");
            //Act
            var test = await register.RegisterUser(username, "password", userhash);
            var expected = await testDBO.SelectUserAccount(username);
            var userAccount = expected.data;
            user = new UserProfile(userAccount._userID, "", "", "", System.DateTime.UtcNow, new GenericIdentity("Regular User"));
            var result = delMan.DeleteAccount(user, user); //Start the deletion manager 
            //Assert
            Console.WriteLine("Account Deletion Successful" + result.ErrorMessage);
            Assert.IsNotNull(result);
            Assert.IsTrue((int)result.Data > 0);
            Assert.IsTrue(result.IsSuccessful);
        }
        [TestMethod]
        public async Task CanAdminDeleteOtherAdminAccountAsync()
        {
            //Arrange
            IDBSelecter testDBO = new SqlDAO(usersString);
            IDAO featDBO = new SqlDAO(usersString);
            // Manual DI
            var userDAO = new UsersSqlDAO(usersString);
            var featureDAO = new SqlDAO(featureString);
            var hashDAO = new UserhashSqlDAO(hashString);
            var logDAO = new LogsSqlDAO(logString);
            var reg = new AccountRegisterer(userDAO, userDAO);
            var hash = new UserhashServices(hashDAO);
            var auth = new AccountAuthentication(userDAO);
            var rec = new RecoveryServices(userDAO, userDAO, userDAO);
            ILogger logger = new Logger(new LogsSqlDAO(logString));
            var register = new SecurityManager(reg, hash, auth, rec, logger);

            var username = "Abel" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(4)) + "@yahoo.com";
            var user = new UserProfile();
            var delMan = new DeletionManager();
            var userhash = SecureHasher.HashString(username, "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI");
            //Act
            var test = await register.RegisterUser(username, "password", userhash);
            var expected = await testDBO.SelectUserAccount(username);
            var userAccount = expected.data;
            user = new UserProfile(userAccount._userID, "", "", "", System.DateTime.UtcNow, new GenericIdentity(userAccount._userID.ToString(), "Admin User"));
            var admin = new UserProfile(new GenericIdentity("1001", "Admin User"));
            var result = delMan.DeleteAccount(user, admin); //Start the deletion manager 
            //Assert
            Console.WriteLine("Account Deletion Successful" + result.ErrorMessage);
            Assert.IsNotNull(result);
            Assert.IsTrue((int)result.Data > 0);
            Assert.IsTrue(result.IsSuccessful);
        }
        [TestMethod]
        public async Task CanAdminDeleteUserAccountAsync()
        {
            //Arrange
            IDBSelecter testDBO = new SqlDAO(usersString);
            IDAO featDBO = new SqlDAO(usersString);
            // Manual DI
            var userDAO = new UsersSqlDAO(usersString);
            var featureDAO = new SqlDAO(featureString);
            var hashDAO = new UserhashSqlDAO(hashString);
            var logDAO = new LogsSqlDAO(logString);
            var reg = new AccountRegisterer(userDAO, userDAO);
            var hash = new UserhashServices(hashDAO);
            var auth = new AccountAuthentication(userDAO);
            var rec = new RecoveryServices(userDAO, userDAO, userDAO);
            ILogger logger = new Logger(new LogsSqlDAO(logString));
            var register = new SecurityManager(reg, hash, auth, rec, logger);

            var username = "Abel" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(4)) + "@yahoo.com";
            var user = new UserProfile();
            var delMan = new DeletionManager();
            var userhash = SecureHasher.HashString(username, "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI");
            //Act
            var test = await register.RegisterUser(username, "password", userhash);
            var expected = await testDBO.SelectUserAccount(username);
            var userAccount = expected.data;
            user = new UserProfile(userAccount._userID, "", "", "", System.DateTime.UtcNow, new GenericIdentity(userAccount._userID.ToString(), "Regular User"));
            var admin = new UserProfile(new GenericIdentity("1001", "Admin User"));
            var result = delMan.DeleteAccount(user, admin); //Start the deletion manager 
            //Assert
            Console.WriteLine("Account Deletion Successful" + result.ErrorMessage);
            Assert.IsNotNull(result);
            Assert.IsTrue((int)result.Data > 0);
            Assert.IsTrue(result.IsSuccessful);
        }*/
    }
}
