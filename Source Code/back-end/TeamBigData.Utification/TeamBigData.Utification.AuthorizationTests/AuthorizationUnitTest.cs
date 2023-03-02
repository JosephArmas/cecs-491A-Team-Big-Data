using System.Security.Principal;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.AuthorizationTests
{
    [TestClass]
    public class AuthorizationUnitTest
    {
        [TestMethod]
        public void CheckRoleAdmin()
        {
            //Arrange
            var sysUnderTest = new UserProfile(new GenericIdentity("username", "Admin User"));
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
            var sysUnderTest = new UserProfile(new GenericIdentity("username", "Regular User"));
            //Act
            var logResult = ((IPrincipal)sysUnderTest).IsInRole("Regular User");
            //Assert
            Console.WriteLine("Is an Regular User: " + logResult);
            Assert.IsTrue(logResult);
        }
    }
}