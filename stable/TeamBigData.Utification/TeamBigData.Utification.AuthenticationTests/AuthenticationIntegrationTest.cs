using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.AccountServices;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;

namespace TeamBigData.Utification.AuthenticationTests
{
    [TestClass]
    public class AuthenticationIntegrationTest
    {
        [TestMethod]
        public void SucessfullyLogin()
        {
            //Arrange
            var result = new Response();
            var securityManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var password = "password";
            //Act
            var digest = encryptor.encryptString(password);
            result = securityManager.VerifyUser(username, digest, encryptor).Result;
            var message = securityManager.SendOTP();
            var result2 = securityManager.VerifyOTP(message);
            //Assert
            Assert.IsTrue(result.isSuccessful);
            Assert.IsTrue(securityManager.IsAuthenticated());
        }

        [TestMethod]
        public void CantLoginWhileAlreadyLoggedIn()
        {
            //Arrange
            var result = new Response();
            var expected = "Error You are already Logged In";
            var securityManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var password = "password";
            //Act
            var digest = encryptor.encryptString(password);
            result = securityManager.VerifyUser(username, digest, encryptor).Result;
            var message = securityManager.SendOTP();
            var result2 = securityManager.VerifyOTP(message);
            //Verify User is truly authenticated
            Assert.IsTrue(securityManager.IsAuthenticated());

            result = securityManager.VerifyUser(username, digest, encryptor).Result;
            //Assert
            Assert.IsFalse(result.isSuccessful);
            Assert.AreEqual(result.errorMessage, expected);
        }

        [TestMethod]
        public void FailsWhenWrongOTPEntered()
        {
            //Arrange
            var result = new Response();
            var securityManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var password = "password";
            //Act
            var digest = encryptor.encryptString(password);
            result = securityManager.VerifyUser(username, digest, encryptor).Result;
            var result2 = securityManager.VerifyOTP("wrongOTP");
            //Assert
            Assert.IsFalse(result2.isSuccessful);
            Assert.IsFalse(securityManager.IsAuthenticated());
        }

        [TestMethod]
        public void AccountDisabledAfter3Attempts()
        {
            //Arrange
            var result = new Response();
            var securityManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "disabledUser@yahoo.com";
            var password = "wrongPassword";
            //Act
            var digest = encryptor.encryptString(password);
            securityManager.VerifyUser(username, digest, encryptor);
            securityManager.VerifyUser(username, digest, encryptor);
            result = securityManager.VerifyUser(username, digest, encryptor).Result;
            //Assert
            Assert.IsFalse(result.isSuccessful);
            Assert.IsTrue(result.errorMessage.Contains("disabled"));
        }

        [TestMethod]
        public void CantLoginWhenDisabled() 
        {
            //Arrange
            var result = new Response();
            var securityManager = new SecurityManager();
            var encryptor = new Encryptor();
            var sqlDAO = new SqlDAO(@"Server=localhost;port=3306;Database=dbo;Uid=root;Pwd=user");
            var enabler = new AccountDisabler(sqlDAO);
            var username = "disabledUser@yahoo.com";
            var password = "password";
            var sysUnderTest = new Logger(new SqlDAO(@"Server=localhost;port=3306;Database=dbo;Uid=root;Pwd=user;"));
            var log = new Log(1, "Info", "SYSTEM", "CantLoginWhenDisabled", "Data", "This is a automated test");

            //Act
            enabler.DisableAccount("disabledUser@yahoo.com");
            var digest = encryptor.encryptString(password);
            result = securityManager.VerifyUser(username, digest, encryptor).Result;
            var rows = sysUnderTest.Log(log).Result;

            //Assert
            Assert.IsFalse(result.isSuccessful);
            Assert.IsTrue(result.errorMessage == "Error: Account disabled. Perform account recovery or contact system admin");
        }

        [TestMethod]
        public void CantLoginWhenDisabledLogFail()
        {
            //Arrange
            var result = new Response();
            var securityManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "disabledUser@yahoo.com";
            var password = "password";
            var sysUnderTest = new Logger(new SqlDAO(@"Server=localhost;port=3306;Database=dbo;Uid=root;Pwd=user;"));
            var log = new Log(1, "Error", "WrongInfo", "CantLoginWhenDisabledLogFail", "View", "This is a automated test");

            //Act
            var digest = encryptor.encryptString(password);
            result = securityManager.VerifyUser(username, digest, encryptor).Result;
            var rows = sysUnderTest.Log(log).Result;

            //Assert
            Assert.IsTrue(result.errorMessage == "Error: Account disabled. Perform account recovery or contact system admin");
            Assert.IsFalse(result.isSuccessful);
        }
    }
}
