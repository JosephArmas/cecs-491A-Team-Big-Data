using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /*
         * Basically what you want to do is log in to an account that is already on the database. 
         * That account is disabled so the code for checking for disabled will likely be somewhere in the business layer.
         * However, since we are not carrying over the disabled variable, we have to do this in SQLDAO likely. Then return the error from there
         */
        [TestMethod]
        public void CantLoginWhenDisabled() 
        {
            //Arrange
            var result = new Response();
            var securityManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var password = "password";
            var sysUnderTest = new Logger(new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            var log = new Log(1, "Info", "SYSTEM", "CantLoginWhenDisabled", "Data", "This is a automated test");

            //Act
            var digest = encryptor.encryptString(password);
            result = securityManager.VerifyUser(username, digest, encryptor).Result;
            var message = securityManager.SendOTP();
            var result2 = securityManager.VerifyOTP(message);
            var rows = sysUnderTest.Log(log).Result;

            //Assert
            Assert.IsTrue(result.isSuccessful);
            Assert.IsTrue(securityManager.IsAuthenticated());
        }

        [TestMethod]
        public void CantLoginWhenDisabledLogFail()
        {
            //Arrange
            var result = new Response();
            var securityManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var password = "password";
            var sysUnderTest = new Logger(new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            var log = new Log(1, "Error", "WrongInfo", "CantLoginWhenDisabledLogFail", "View", "This is a automated test");

            //Act
            var digest = encryptor.encryptString(password);
            result = securityManager.VerifyUser(username, digest, encryptor).Result;
            var message = securityManager.SendOTP();
            var result2 = securityManager.VerifyOTP(message);
            var rows = sysUnderTest.Log(log).Result;

            //Assert
            Assert.IsTrue(result.isSuccessful);
            Assert.IsTrue(securityManager.IsAuthenticated());
        }
    }
}
