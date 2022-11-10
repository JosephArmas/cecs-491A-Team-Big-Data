using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.Logging;

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

            //Act
            var logResult = sysUnderTest.Log("INSERT INTO dbo.Logs (CorrelationID,LogLevel,[User],[DateTime],[Event],Category,[Message]) VALUES (1, 'Trace','SYSTEM','" + DateTime.UtcNow.ToString() + "', 'DAO_LogMustSaveToDataStore', 'Data','This is a automated test')");
            //Assert
            Console.WriteLine(logResult.Result.errorMessage);
            Assert.IsFalse(logResult.Result.isSuccessful);
        }
        [TestMethod]
        public void SQL_LogHasWrongCategory()
        {
            //Arrange
            var sysUnderTest = new Logger(new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));

            //Act
            var logResult = sysUnderTest.Log("INSERT INTO dbo.Logs (CorrelationID,LogLevel,[User],[DateTime],[Event],Category,[Message]) VALUES (1, 'Info','SYSTEM','" + DateTime.UtcNow.ToString() + "', 'DAO_LogMustSaveToDataStore', 'Type','This is a automated test')");
            //Assert
            Console.WriteLine(logResult.Result.errorMessage);
            Assert.IsFalse(logResult.Result.isSuccessful);
        }
        [TestMethod]
        public void SQL_LogIsNotInsert()
        {
            //Arrange
            var sysUnderTest = new Logger(new SqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));

            //Act
            var logResult = sysUnderTest.Log("UPDATE dbo.Logs SET Message = 'Updated' WHERE LogID = 1");
            //Assert
            Console.WriteLine(logResult.Result.errorMessage);
            Assert.IsFalse(logResult.Result.isSuccessful);
        }
    }
}