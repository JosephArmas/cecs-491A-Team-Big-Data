using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.AccountServices;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.DeletionService;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB;
using TeamBigData.Utification.SQLDataAccess.LogsDB;
using TeamBigData.Utification.SQLDataAccess.UserhashDB;
using TeamBigData.Utification.SQLDataAccess.UsersDB;

namespace TeamBigData.Utification.AuthenticationTests
{
    [TestClass]
    public class AuthenticationIntegrationTest
    {
        private IServiceCollection _services;
        private IServiceProvider _provider;

        public AuthenticationIntegrationTest()
        {
            // Arrange
            _services = new ServiceCollection();

            _services.AddDbContext<LogsSqlDAO>(options => options.UseSqlServer("Server=.\\;Database=TeamBigData.Utification.Logs;User=AppUser; Password=t; TrustServerCertificate=True; Encrypt=True"));
            _services.AddTransient<ILogger, Logger>();

            _services.AddDbContext<PinsSqlDAO>(options => options.UseSqlServer("Server=.\\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False"));
            

            _services.AddDbContext<UsersSqlDAO>(options => options.UseSqlServer("Server=.\\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False"));
            _services.AddTransient<AccountRegisterer>();
            _services.AddTransient<AccountAuthentication>();

            _services.AddTransient<RecoveryServices>();

            _services.AddDbContext<UserhashSqlDAO>(options => options.UseSqlServer("Server=.\\;Database=TeamBigData.Utification.UserHash;Integrated Security=True;Encrypt=False"));
            _services.AddTransient<UserhashServices>();

            _services.AddTransient<AccDeletionService>();

            _services.AddTransient<SecurityManager>();


            _provider = _services.BuildServiceProvider();
        }

        [TestMethod]
        public async Task SucessfullyLogin()
        {
            //Arrange
            var sysUnderTest = _provider.GetRequiredService<SecurityManager>();
            var username = "testUser@yahoo.com";
            var password = "password";
            var userhash = SecureHasher.HashString("5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI", username);


            //Act
            var result = await sysUnderTest.LoginUser(username, password, userhash);

            //Assert
            Assert.IsTrue(result.isSuccessful);
        }

        [TestMethod]
        public async Task FailsWhenWrongOTPEntered()
        {
            //Arrange
            var sysUnderTest = _provider.GetRequiredService<SecurityManager>();
            var username = "testUser@yahoo.com";
            var password = "password";
            var userhash = SecureHasher.HashString("5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI", username);
            var invalidOTP = "wrongOTP";
            //Act
            var result = await sysUnderTest.LoginUser(username, password, userhash);
            //var result2 = securityManager.LoginOTP("wrongOTP");
            //Assert
            Assert.AreNotEqual(result.data._otp, invalidOTP);
            Assert.IsTrue(result.isSuccessful);
        }

        [TestMethod]
        public async Task AccountDisabledAfter3Attempts()
        {
            //Arrange
            var sysUnderTest = _provider.GetRequiredService<SecurityManager>();
            var username = "disabledUser@yahoo.com";
            var password = "wrongPassword";
            var userhash = SecureHasher.HashString("5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI", username);

            //Act
            await sysUnderTest.LoginUser(username, password, userhash);
            await sysUnderTest.LoginUser(username, password, userhash);
            var result = await sysUnderTest.LoginUser(username, password, userhash);
            
            //Assert
            Assert.IsFalse(result.isSuccessful);
            Assert.IsTrue(result.errorMessage.Contains("Error: Account disabled. Perform account recovery or contact system admin"));
        }
        /*
        [TestMethod]
        public async Task CantLoginWhenDisabled()
        {
            //Arrange
            var securityManager = new SecurityManager();
            var encryptor = new Encryptor();
            var sqlDAO = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False");
            var enabler = new AccountDisabler(sqlDAO);
            var username = "disabledUser@yahoo.com";
            var password = "password";
            UserAccount userAccount = new UserAccount();
            UserProfile userProfile = new UserProfile();
            var sysUnderTest = new Logger(new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            var log = new Log(1, "Info", "SYSTEM", "CantLoginWhenDisabled", "Data", "This is a automated test");

            //Act
            var response = enabler.DisableAccount("disabledUser@yahoo.com");
            var digest = encryptor.encryptString(password);
            var result = await securityManager.LoginUser(username, digest, encryptor, userProfile);
            var rows = sysUnderTest.Log(log).Result;

            //Assert
            Assert.IsFalse(result.isSuccessful);
            Assert.IsTrue(result.errorMessage == "Error: Account disabled. Perform account recovery or contact system admin");
        }

        [TestMethod]
        public async Task CantLoginWhenDisabledLogFail()
        {
            //Arrange
            var securityManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "disabledUser@yahoo.com";
            var password = "password";
            UserAccount userAccount = new UserAccount();
            UserProfile userProfile = new UserProfile();
            var sysUnderTest = new Logger(new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            var log = new Log(1, "Error", "WrongInfo", "CantLoginWhenDisabledLogFail", "View", "This is a automated test");

            //Act
            var digest = encryptor.encryptString(password);
            var result = await securityManager.LoginUser(username, digest, encryptor, userProfile);
            var rows = sysUnderTest.Log(log).Result;

            //Assert
            Assert.IsTrue(result.errorMessage == "Error: Account disabled. Perform account recovery or contact system admin");
            Assert.IsFalse(result.isSuccessful);
        }*/
    }
}
