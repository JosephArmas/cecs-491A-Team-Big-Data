using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using TeamBigData.Utification.AccountServices;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;

namespace TeamBigData.Utification.AccountDeletionTests
{
    [TestClass]
    public class AccountDeletionIntegrationTest
    {
        [TestMethod]
        public void CanDeleteOwnAccount()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            var featureConnectionString = @"Server=.\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False";
            SqlDAO testuserDBO = new SqlDAO(connectionString);
            SqlDAO testfeatDBO = new SqlDAO(featureConnectionString);
            var user = new UserProfile(new GenericIdentity("Deletius", "Regular User")); //Create a fake user profile to see if it will allow the deletion
            var delMan = new DeletionManager();
            //Act
            testuserDBO.Execute("INSERT INTO dbo.Users (username, \"disabled\") VALUES ('Deletius',0);INSERT INTO dbo.UserProfiles (username, role) VALUES ('Deletius','Regular User');");
            testfeatDBO.Execute("INSERT INTO dbo.Pins VALUES ('Deletius',0);\r\nINSERT INTO dbo.\"Events\" VALUES ('Deletius',0);\r\nINSERT INTO dbo.\"Services\" VALUES ('Deletius',0);\r\nINSERT INTO dbo.Pictures VALUES ('Deletius',0);");
            var result = delMan.DeleteAccount(user.Identity.Name, user); //Start the deletion manager 
            //Assert
            Console.WriteLine(result.errorMessage + result.isSuccessful + result.data);
            Assert.IsNotNull(result);
            Assert.IsTrue((int)result.data > 0);
            Assert.IsTrue(result.isSuccessful);
        }
        [TestMethod]
        public void CanAdminDeleteOtherAdminAccount()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            var featureConnectionString = @"Server=.\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False";
            SqlDAO testuserDBO = new SqlDAO(connectionString);
            SqlDAO testfeatDBO = new SqlDAO(featureConnectionString);
            var adUser = new UserProfile(new GenericIdentity("Kratos", "Admin User")); //Create first admin user to attempt deletion from
            var vicUser = new UserProfile(new GenericIdentity("Ares", "Admin User")); //Create second admin user to be deleted
            var delMan = new DeletionManager();
            //Act
            testuserDBO.Execute("INSERT INTO dbo.Users (username, \"disabled\") VALUES ('Ares',0);INSERT INTO dbo.UserProfiles (username, role) VALUES ('Ares','Admin User');");
            testfeatDBO.Execute("INSERT INTO dbo.Pins VALUES ('Ares',0);\r\nINSERT INTO dbo.\"Events\" VALUES ('Ares',0);\r\nINSERT INTO dbo.\"Services\" VALUES ('Ares',0);\r\nINSERT INTO dbo.Pictures VALUES ('Ares',0);");
            var result = delMan.DeleteAccount(vicUser.Identity.Name, adUser);
            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue((int)result.data > 0);
            Assert.IsTrue(result.isSuccessful);
        }
        [TestMethod]
        public void CanAdminDeleteUserAccount()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            var featureConnectionString = @"Server=.\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False";
            SqlDAO testuserDBO = new SqlDAO(connectionString);
            SqlDAO testfeatDBO = new SqlDAO(featureConnectionString);
            var adUser = new UserProfile(new GenericIdentity("God", "Admin User")); //Create admin user to attemot deletion from
            var vicUser = new UserProfile(new GenericIdentity("Sodom", "Regular User")); //Create regular user to be deleted
            var delMan = new DeletionManager();
            //Act
            testuserDBO.Execute("INSERT INTO dbo.Users (username, \"disabled\") VALUES ('Sodom',0);INSERT INTO dbo.UserProfiles (username, role) VALUES ('Sodom','Regular User');");
            testfeatDBO.Execute("INSERT INTO dbo.Pins VALUES ('Sodom',0);\r\nINSERT INTO dbo.\"Events\" VALUES ('Sodom',0);\r\nINSERT INTO dbo.\"Services\" VALUES ('Sodom',0);\r\nINSERT INTO dbo.Pictures VALUES ('Sodom',0);");
            var result = delMan.DeleteAccount(vicUser.Identity.Name, adUser);
            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue((int)result.data > 0);
            Assert.IsTrue(result.isSuccessful);
        }
    }
}
