using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.LogsDB;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Models;
using System.Diagnostics;

namespace TeamBigData.Utification.LoggingTests
{
    /*[TestClass]
    public class DataAccessTest
    {
        [TestMethod]
        public async Task DAO_LogMustSaveToDataStore() //If updating the data store make sure to assert each individual column for maximum verification
        {
            //Arrange
            var sysUnderTest = new Logger(new LogsSqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            var log = new Log(1, "Info", "SYSTEM", "DAO_LogMustSaveToDataStore", "Data", "This is a automated test");
            //Act
            var rows = await sysUnderTest.Logs(log);
            //Assert
            Assert.IsTrue(rows.IsSuccessful);
        }
        [TestMethod]
        public async Task DAO_LogMustBeImmutable()
        {
            //Arrange
            var sysUnderTest = new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True");
            var updateSql = "UPDATE dbo.Logs SET Message = 'Updated' WHERE LogID = 1";
            bool check = false; //A check value used to determine if the Command successfully recieves an error.
            //Act
            var rows = await sysUnderTest.Execute(updateSql);
            //Assert
            Assert.IsFalse(rows.IsSuccessful);
        }
        [TestMethod]
        public async Task DAO_MustLogWithin5Secs()
        {
            //Arrange
            var stopwatch = new Stopwatch();
            var expected = 5000;
            var sysUnderTest = new Logger(new LogsSqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            var log = new Log(2, "Info", "SYSTEM", "DAO_MustLogWithin5Secs", "Business", "This is a automated test for finding if it took longer than 5 seconds");
            //Act
            stopwatch.Start();
            var logResult = await sysUnderTest.Logs(log);
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;
            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(logResult.IsSuccessful);
        }
    }*/
}