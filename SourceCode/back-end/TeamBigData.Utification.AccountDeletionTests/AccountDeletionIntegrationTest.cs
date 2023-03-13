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
using TeamBigData.Utification.SQLDataAccess.Abstractions;

namespace TeamBigData.Utification.AccountDeletionTests
{
    [TestClass]
    public class AccountDeletionIntegrationTest
    {
        [TestMethod]
        public async Task CanDeleteOwnAccountAsync()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            IDBSelecter testDBO = new SqlDAO(connectionString);
            IDAO featDBO = new SqlDAO(connectionString);
            IRegister register = new SecurityManager();
            UserAccount userAccount = new UserAccount();
            var username = "Deletius" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(4)) + "@yahoo.com";
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            var user = new UserProfile();
            var delMan = new DeletionManager();
            //Act
            var test = await register.RegisterUser(username, encryptedPassword, encryptor);
            var expected = await testDBO.SelectUserAccount(ref userAccount, username);
            user = new UserProfile(userAccount._userID, "", "", "", System.DateTime.UtcNow, new GenericIdentity("Regular User"));
            var result = delMan.DeleteAccount(user, user); //Start the deletion manager 
            //Assert
            Console.WriteLine("Account Deletion Successful"+result.data+result.errorMessage);
            Assert.IsNotNull(result);
            Assert.IsTrue((int)result.data > 0);
            Assert.IsTrue(result.isSuccessful);
        }
        [TestMethod]
        public async Task CanAdminDeleteOtherAdminAccountAsync()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            IDBSelecter testDBO = new SqlDAO(connectionString);
            IDAO featDBO = new SqlDAO(connectionString);
            IRegister register = new SecurityManager();
            UserAccount userAccount = new UserAccount();
            UserAccount adUserAccount = new UserAccount();
            var username = "Zeus" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(4)) + "@yahoo.com";
            UserAccount adminAccount = new UserAccount();
            var adUsername = "Ares" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(4)) + "@yahoo.com";
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            var vicUser = new UserProfile();
            var adUser = new UserProfile();
            var delMan = new DeletionManager();
            //Act
            var admin = await register.RegisterUser(adUsername, encryptedPassword, encryptor);
            var victim = await register.RegisterUser(username, encryptedPassword, encryptor);
            var getVic = await testDBO.SelectUserAccount(ref userAccount, username);
            vicUser = new UserProfile(userAccount._userID, "", "", "", System.DateTime.UtcNow, new GenericIdentity("Admin User"));
            var getAd = await testDBO.SelectUserAccount(ref adUserAccount, adUsername);
            adUser = new UserProfile(adUserAccount._userID, "", "", "", System.DateTime.UtcNow, new GenericIdentity("Admin User"));
            var result = delMan.DeleteAccount(vicUser, adUser);
            //Assert
            Console.WriteLine("Account Deletion Successful");
            Assert.IsNotNull(result);
            Assert.IsTrue((int)result.data > 0);
            Assert.IsTrue(result.isSuccessful);
        }
        [TestMethod]
        public async Task CanAdminDeleteUserAccountAsync()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            var featconnectionString = @"Server=.\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False";
            IDBSelecter testDBO = new SqlDAO(connectionString);
            IDAO featDBO = new SqlDAO(featconnectionString);
            IRegister register = new SecurityManager();
            UserAccount userAccount = new UserAccount();
            UserAccount adUserAccount = new UserAccount();
            var username = "Sodom" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(4)) + "@yahoo.com";
            UserAccount adminAccount = new UserAccount();
            var adUsername = "GodAlmighty" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(4)) + "@yahoo.com";
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString("password");
            var vicUser = new UserProfile();
            var adUser = new UserProfile();
            var delMan = new DeletionManager();
            //Act
            var admin = await register.RegisterUser(adUsername, encryptedPassword, encryptor);
            var victim = await register.RegisterUser(username, encryptedPassword, encryptor);
            var getVic = await testDBO.SelectUserAccount(ref userAccount, username);
            vicUser = new UserProfile(userAccount._userID, "", "", "", System.DateTime.UtcNow, new GenericIdentity("Regular User"));
            var getAd = await testDBO.SelectUserAccount(ref adUserAccount, adUsername);
            adUser = new UserProfile(adUserAccount._userID, "", "", "", System.DateTime.UtcNow, new GenericIdentity("Admin User"));
            var result = delMan.DeleteAccount(vicUser, adUser);
            //Assert
            Console.WriteLine("Account Deletion Successful");
            Assert.IsNotNull(result);
            Assert.IsTrue((int)result.data > 0);
            Assert.IsTrue(result.isSuccessful);
        }
    }
}
