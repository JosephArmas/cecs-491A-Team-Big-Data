using DataAccess;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using System.Data;
using System.Diagnostics;

namespace LoggingUnitTest
{
    [TestClass]
    public class DataAccessTest
    {
        [TestMethod]
        public void LogMustSaveToDataStore() //If updating the data store make sure to assert each individual column for maximum verification
        {
            //Arrange
            var sysUnderTest = new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User Id=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True");

            Random rnd = new Random();
            int intmsg = rnd.Next();
            string msg = intmsg.ToString();
            var insertSql = "INSERT INTO dbo.Logs (Message) VALUES ('" + msg + "')";
            //var selectSql = "SELECT [Message] FROM dbo.Logs WHERE Message = '" + msg + "'";
            //Act
            var rows = sysUnderTest.Execute(insertSql);
            //var rowsaved = sysUnderTest.Execute(selectSql);
            //Assert
            //Assert.IsTrue(rows == 1);
            //Console.WriteLine(msg);
            //Console.WriteLine(rowsaved. + "==" + msg);
            Assert.IsTrue(rows.IsSuccessful);
            //Assert.IsTrue(rowsaved.Payload.Equals(msg));
        }
        [TestMethod]
        public void LogMustBeImmutable()
        {
            //Arrange
            var sysUnderTest = new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User Id=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True");
            var updateSql = "UPDATE dbo.Logs SET Message = 'Updated' WHERE LogID = 24";
            bool check = false; //A check value used to determine if the Command successfully recieves an error.
            //Act
            try
            {
                var rows = sysUnderTest.Execute(updateSql);
            }
            catch (Exception ex)
            {
                Console.WriteLine("A " + ex.GetType().ToString() + " has occured."); // Catches the exception to the ability to update on this user account. A error is a positive result
                check = true;
            }
            //Assert
            Assert.IsTrue(check);
        }
        [TestMethod]
        public void MustLogWithin5Secs()
        {
            //Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var sysUnderTest = new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User Id=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True");
            var insertSql = "INSERT INTO dbo.Logs (Message) VALUES ('Test')";
            //Act
            stopwatch.Start();
            var logResult = sysUnderTest.Execute(insertSql);
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds / 1000; //Turn ms to seconds
            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(logResult.IsSuccessful);
        }
        [TestMethod]
        public void TestMethod1()
        {
            //Arrange

            //Act

            //Assert

        }
    }
}