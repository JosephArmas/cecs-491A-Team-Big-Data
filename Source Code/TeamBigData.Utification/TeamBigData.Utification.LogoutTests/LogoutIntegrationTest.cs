using System.Diagnostics;
using System.Security.Cryptography;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;

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
            var userAccount = new UserAccount();
            var userProfile = new UserProfile();
            var username = "SuccessfulLogoutTest"+ Convert.ToBase64String(RandomNumberGenerator.GetBytes(4)) + "@yahoo.com";
            var password = "password";
            //Act
            stopwatch.Start();
            var digest = encryptor.encryptString(password);
            result = securityManager.RegisterUser(username, digest, encryptor).Result;
            result = securityManager.LoginUser(username, digest, encryptor, ref userAccount, ref userProfile).Result;
            result = securityManager.LogOutUser(ref userAccount, ref userProfile).Result;
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;
            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual <= expected);
            Console.WriteLine(actual <= expected);
            Assert.IsTrue(actual >= 0);
            Console.WriteLine(actual >= 0);
            Assert.IsTrue(result.isSuccessful);
            Console.WriteLine(result.isSuccessful);
        }
    }
}