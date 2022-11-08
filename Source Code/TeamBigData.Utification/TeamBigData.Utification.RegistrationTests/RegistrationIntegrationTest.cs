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
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            SqlDAO testDBO = new SqlDAO(connectionString);
            Registerer testRegister = new Registerer(testDBO);
            String password = "password";
            String email = "dwaviddg@yahoo.com";
            //Act
            testDBO.Clear("dbo.TestUsers");
            var actual = testRegister.InsertUser("dbo.TestUsers", email, password);
            //Assert
            Assert.IsTrue(actual.isSuccessful);
        }

        [TestMethod]
        public void ClearTestUsersWorks()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            SqlDAO testDBO = new SqlDAO(connectionString);
            //Act
            var actual = testDBO.Clear("dbo.TestUsers");
            //Assert
            Assert.IsTrue(actual.isSuccessful);
        }

        [TestMethod]
        public void CatchesDuplicateEmail()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            SqlDAO testDBO = new SqlDAO(connectionString);
            Registerer testRegister = new Registerer(testDBO);
            String password = "password";
            String email = "daviddg5@yahoo.com";
            //Act
            testDBO.Clear("dbo.TestUsers");
            testRegister.InsertUser("dbo.TestUsers", email, password);
            var actual = testRegister.InsertUser("dbo.TestUsers", email, password);
            //Assert
            Assert.IsTrue(actual.errorMessage.Contains("Email"));
        }

        [TestMethod]
        public void ShouldRegisterWithin5Seconds()
        {
            //Arrange
            Stopwatch stopwatch = new Stopwatch();
            long expected = 5 * 60000;
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            SqlDAO testDBO = new SqlDAO(connectionString);
            Registerer testRegister = new Registerer(testDBO);
            String username = "daviddg5";
            String password = "password";
            String email = "daviddg5@yahoo.com";
            //Act
            testDBO.Clear("dbo.TestUsers");
            stopwatch.Start();
            var result = testRegister.InsertUser("dbo.TestUsers", email, password);
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;

            //Assert
            Assert.IsTrue(actual < expected);
            Assert.IsTrue(result.isSuccessful);
        }
    }
}
