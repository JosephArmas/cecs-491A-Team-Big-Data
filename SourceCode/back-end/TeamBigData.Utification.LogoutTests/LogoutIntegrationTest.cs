using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Principal;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.LogoutTests
{
    /*[TestClass]
    public class LogoutIntegrationTest
    {
        [TestMethod]
        public async Task SuccessfullyLogout()
        {
            //Arrange
            var stopwatch = new Stopwatch();
            var expected = 5000;
            var securityManager = new SecurityManager();
            var userProfile = new UserProfile(new GenericIdentity("Regular User"));
            //Act
            stopwatch.Start();
            var result3 = await securityManager.LogOutUser(userProfile, "Logout Test");
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;
            Console.WriteLine(result3.ErrorMessage);
            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(result3.IsSuccessful);
        }
    }*/
}