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
            String username = "daviddg5";
            String password = "password";
            String email = "dwaviddg@yahoo.com";
            //Act
            testDBO.Clear("dbo.TestUsers");
            var actual = testRegister.InsertUser("dbo.TestUsers", username, password, email);
            //Assert
            Assert.IsTrue(actual.isSuccessful);
        }

        [TestMethod]
        public void ClearTestUsersWork()
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
        public void CatchesPrimaryKeyFault()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            SqlDAO testDBO = new SqlDAO(connectionString);
            Registerer testRegister = new Registerer(testDBO);
            String username = "daviddg5";
            String password = "password";
            String email = "daviddg5@yahoo.com";
            String username2 = "daviddg10";
            //Act
            testDBO.Clear("dbo.TestUsers");
            testRegister.InsertUser("dbo.TestUsers", username, password, email);
            var actual = testRegister.InsertUser("dbo.TestUsers", username2, password, email);
            //Assert
            Assert.IsTrue(actual.errorMessage.Contains("Email"));
        }


        [TestMethod]
        public void CatchesCandidateKeyFault()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            SqlDAO testDBO = new SqlDAO(connectionString);
            Registerer testRegister = new Registerer(testDBO);
            Console.WriteLine(testDBO);
            Console.WriteLine(testRegister);
            String username = "daviddg5";
            String password = "password";
            String email = "daviddg5@yahoo.com";
            String email2 = "daviddg@yahoo.com";
            //Act
            testDBO.Clear("dbo.TestUsers");
            testRegister.InsertUser("dbo.TestUsers", username, password, email);
            var actual = testRegister.InsertUser("dbo.TestUsers", username, password, email2);
            //Assert
            Assert.IsTrue(actual.errorMessage.Contains("Username"));
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
            var result = testRegister.InsertUser("dbo.TestUsers", username, password, email);
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;

            //Assert
            Assert.IsTrue(actual < expected);
            Assert.IsTrue(result.isSuccessful);
        }
    }
}
