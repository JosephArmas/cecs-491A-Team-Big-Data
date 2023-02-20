using System.Diagnostics;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Manager;

namespace TeamBigData.Utification.LogoutTests
{
    [TestClass]
    public class LogoutIntegrationTest
    {
        [TestMethod]
        public void SuccessfullyLogout()
        {
            //Arrange
            var stopwatch = new Stopwatch();
            var expected = 5000;
            var result = new Response();
            var securityManager = new SecurityManager();
            var encryptor = new Encryptor();
            var username = "testUser@yahoo.com";
            var password = "password";
            //Act
            stopwatch.Start();
            var digest = encryptor.encryptString(password);
            result = securityManager.VerifyUser(username, digest, encryptor).Result;
            var message = securityManager.SendOTP();
            var result2 = securityManager.VerifyOTP(message);
            result = securityManager.LogOut();
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;
            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(result.isSuccessful);
        }


    }
}