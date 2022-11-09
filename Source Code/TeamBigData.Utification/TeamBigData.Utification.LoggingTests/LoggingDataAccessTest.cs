using TeamBigData.Utification.ErrorResponse;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using System.Data;
using System.Diagnostics;
using TeamBigData.Utification.SQLDataAccess;

namespace TeamBigData.Utification.LoggingTest
{
    [TestClass]
    public class DataAccessTest
    {
        [TestMethod]
        public void DAO_LogMustSaveToDataStore() //If updating the data store make sure to assert each individual column for maximum verification
        {
            //Arrange
            var sysUnderTest = new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True");
            var insertSql = "INSERT INTO dbo.Loggem (CorrelationID,LogLevel,[User],[DateTime],[Event],Category,[Message]) VALUES (1, 'Info','SYSTEM','" + DateTime.UtcNow.ToString() + "', 'DAO_LogMustSaveToDataStore', 'Data','This is a automated test')";
            //Act
            var rows = sysUnderTest.Execute(insertSql);
            //Assert
            Assert.IsTrue(rows.Result.isSuccessful);
        }
        [TestMethod]
        public void DAO_LogMustBeImmutable()
        {
            //Arrange
            var sysUnderTest = new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True");
            var updateSql = "UPDATE dbo.Loggem SET Message = 'Updated' WHERE LogID = 1";
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
        public void DAO_MustLogWithin5Secs()
        {
            //Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var sysUnderTest = new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True");
            var insertSql = "INSERT INTO dbo.Loggem (CorrelationID,LogLevel,[User],[DateTime],[Event],Category,[Message]) VALUES (2, 'Info','SYSTEM','" + DateTime.UtcNow.ToString() + "', 'DAO_MustLogWithin5Secs', 'Business','This is a automated test for finding if it took longer than 5 seconds')";
            //Act
            stopwatch.Start();
            var logResult = sysUnderTest.Execute(insertSql);
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds / 1000; //Turn ms to seconds
            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(logResult.Result.isSuccessful);
        }
    }
}