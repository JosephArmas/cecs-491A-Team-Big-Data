using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.LoggingTests
{
    [TestClass]
    public class LoggerTest
    {

        [TestMethod]
        public void SQL_LogHasWrongLogLevel() //Possible needs to be made async
        {
            //Arrange
            var sysUnderTest = new Logger(new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            var log = new Log(1, "Trace", "SYSTEM", "DAO_LogMustSaveToDataStore", "Data", "This is a automated test");
            //Act
            var logResult = sysUnderTest.Log(log);
            //Assert
            Console.WriteLine(logResult.Result.errorMessage);
            Assert.IsFalse(logResult.Result.isSuccessful);
        }
        [TestMethod]
        public void SQL_LogHasWrongCategory()
        {
            //Arrange
            var sysUnderTest = new Logger(new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            var log = new Log(1, "Info", "SYSTEM", "DAO_LogMustSaveToDataStore", "Type", "This is a automated test");
            //Act
            var logResult = sysUnderTest.Log(log);
            //Assert
            Console.WriteLine(logResult.Result.errorMessage);
            Assert.IsFalse(logResult.Result.isSuccessful);
        }
    }
}