using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TeamBigData.Utification.Registration;
using TeamBigData.Utification.SQLDataAccess;

namespace TeamBigData.Utification.RegistrationTests
{
    [TestClass]
    public class RegistrationIntegrationTest
    {
        [TestMethod]
        public void ShouldAddUserToDB()
        {
            //Arrange
            SQLDB testDB = new SQLDB();
            String expected = "No Error";
            String username = "daviddg";
            String password = "password";
            String email = "dwaviddg@yahoo.com";
            //Act
            testDB.ClearTestUsers();
            var actual = testDB.InsertTestUser(username, password, email);
            //Assert
            Assert.AreEqual(expected, actual.errorMessage);
        }

        [TestMethod]
        public void ClearTestUsersWork()
        {
            //Arrange
            SQLDB testDB = new SQLDB();
            //Act
            var actual = testDB.ClearTestUsers();
            //Assert
            Assert.IsTrue(actual.isSuccessful);
        }

        [TestMethod]
        public void CatchesPrimaryKeyFault()
        {
            //Arrange
            SQLDB testDB = new SQLDB();
            String username = "daviddg5";
            String password = "password";
            String email = "daviddg5@yahoo.com";
            String email2 = "daviddg@yahoo.com";
            //Act
            testDB.ClearTestUsers();
            testDB.InsertTestUser(username, password, email);
            var actual = testDB.InsertTestUser(username, password, email2);
            //Assert
            Assert.IsTrue(actual.errorMessage.Contains("Username"));
        }

        [TestMethod]
        public void CatchesCandidateKeyFault()
        {
            //Arrange
            SQLDB testDB = new SQLDB();
            String username = "daviddg5";
            String password = "password";
            String email = "daviddg5@yahoo.com";
            String username2 = "daviddg";
            //Act
            testDB.ClearTestUsers();
            testDB.InsertTestUser(username, password, email);
            var actual = testDB.InsertTestUser(username2, password, email);
            //Assert
            Assert.IsTrue(actual.errorMessage.Contains("Email"));
        }
    }
}
