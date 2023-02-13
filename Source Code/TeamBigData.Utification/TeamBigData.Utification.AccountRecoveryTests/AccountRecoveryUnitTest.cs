using TeamBigData.Utification.SQLDataAccess;
using System.Diagnostics;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using System.Security.Principal;
using TeamBigData.Utification.AccountServices;

namespace TeamBigData.Utification.AccountRecoveryTests
{
    [TestClass]
    public class AccountRecoveryUnitTest
    {
        [TestMethod]
        public void RequestAvailableWithin5Seconds()
        {
            //Arrange
            Stopwatch stopwatch = new Stopwatch();
            SecurityManager secManagerAccRecovery = new SecurityManager();
            long expected = 5 * 1000;
            var userProfile = new UserProfile(new GenericIdentity("username", "Admin User"));
            var requestDB = new SqlDAO(@"Server=.;Database=TeamBigData.Utification.RecoveryRequests;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True");
            string recoveryRequest = "INSERT INTO dbo.RecoveryRequests (username, fulfilled) VALUES ('user1', 0)";
            //Act
            stopwatch.Start();
            var sendRequest = requestDB.Execute(recoveryRequest);           
            List<string> listRequests = new List<string>();
            var result = secManagerAccRecovery.GetRecoveryRequests(listRequests, userProfile);
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;
            //Assert
            Assert.IsNotNull(listRequests);
            Assert.IsTrue(actual < expected);
        }

        [TestMethod]
        public void AdminApprovedAccountRecovery()
        {
            //Arrange
            var requestDB = new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Users;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True");
            SecurityManager adminManager = new SecurityManager();
            var userProfile = new UserProfile(new GenericIdentity("username","Admin User"));
            //Act
            var accountEnabled = adminManager.EnableAccount("user1", userProfile);
            //Assert
            Assert.IsTrue(accountEnabled.isSuccessful);
        }
    }
}