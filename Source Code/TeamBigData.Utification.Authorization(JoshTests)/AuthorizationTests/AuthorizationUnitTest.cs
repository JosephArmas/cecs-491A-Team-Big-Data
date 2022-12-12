using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Authorization.Views;
using TeamBigData.Utification.AuthZ.Abstraction;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;

namespace AuthorizationTests
{
    [TestClass]
    public class AuthorizationUnitTest
    {
        [TestMethod]
        public void CheckRoleAdmin()
        {
            //Arrange
            var sysUnderTest = new UserProfile(new GenericIdentity("username","Admin User"));
            //Act
            var logResult = ((IPrincipal)sysUnderTest).IsInRole("Admin User");
            //Assert
            Console.WriteLine("Is an Admin User: " + logResult);
            Assert.IsTrue(logResult);
        }
        [TestMethod]
        public void CheckRoleAnonymous()
        {
            //Arrange
            var sysUnderTest = new UserProfile(new GenericIdentity("username", "Anonymous User"));
            //Act
            var logResult = ((IPrincipal)sysUnderTest).IsInRole("Anonymous User");
            //Assert
            Console.WriteLine("Is an Anonymous User: " + logResult);
            Assert.IsTrue(logResult);
        }
        [TestMethod]
        public void CheckRoleRegular()
        {
            //Arrange
            var sysUnderTest = new UserProfile(new GenericIdentity("username", "Admin User"));
            //Act
            var logResult = ((IPrincipal)sysUnderTest).IsInRole("Admin User");
            //Assert
            Console.WriteLine("Is an Admin User: " + logResult);
            Assert.IsTrue(logResult);
        }
        /*
        [TestMethod]
        /*
        public void AdminAccessData()
        {
            //Arrange
            var sysUnderTest = new UserProfile(new GenericIdentity("username", "Admin User"));
            var secMan = new SecurityManager();
            var list = new List<UserProfile>();
            //Act
            var logResult = secMan.GetUserProfileTable(list,sysUnderTest);
            //Assert
            Console.WriteLine("Is an Admin User: " + logResult.isSuccessful);
            Console.WriteLine("Can Access Data: " + logResult.isSuccessful);
            Assert.IsTrue(logResult.isSuccessful);
        }
        
        [TestMethod]
        public void RestrictedAccessModification(UserProfile userProfile)
        {
            //Modification is protected
        }
        [TestMethod]
        public void AdminMayNotHaveReputation(UserProfile userProfile)
        {

        }
        [TestMethod]
        public void AdminMayNotHaveUserPermissions(UserProfile userProfile)
        {
            //Admins may not create pins, services, reqeust services, or upload pictures
        }
        */
    }
}
