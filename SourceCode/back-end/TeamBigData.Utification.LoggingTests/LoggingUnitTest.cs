using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TeamBigData.Utification.SQLDataAccess.LogsDB;
using ILogger = TeamBigData.Utification.Logging.Abstraction.ILogger;

namespace TeamBigData.Utification.LoggingTests
{
    [TestClass]
    public class LoggerTest
    {
        private IServiceCollection _services;
        private IServiceProvider _provider;
        public LoggerTest()
        {
            _services = new ServiceCollection();

            _services.AddDbContext<LogsSqlDAO>(options => options.UseSqlServer("Server=.\\;Database=TeamBigData.Utification.Logs;User=AppUser; Password=t; TrustServerCertificate=True; Encrypt=True"));
            _services.AddTransient<ILogger, Logger>();

            _provider = _services.BuildServiceProvider();
        }

        [TestMethod]
        public void SQL_LogHasWrongLogLevel() //Possible needs to be made async
        {
            //Arrange
            var sysUnderTest = _provider.GetRequiredService<ILogger>();
            var log = new Log(1, "Trace", "SYSTEM", "DAO_LogMustSaveToDataStore", "Data", "This is a automated test");
            //Act
            var logResult = sysUnderTest.Logs(log);
            //Assert
            Console.WriteLine(logResult.Result.errorMessage);
            Assert.IsFalse(logResult.Result.isSuccessful);
        }
        [TestMethod]
        public void SQL_LogHasWrongCategory()
        {
            //Arrange
            var sysUnderTest = _provider.GetRequiredService<ILogger>();
            var log = new Log(1, "Info", "SYSTEM", "DAO_LogMustSaveToDataStore", "Type", "This is a automated test");
            //Act
            var logResult = sysUnderTest.Logs(log);
            //Assert
            Console.WriteLine(logResult.Result.errorMessage);
            Assert.IsFalse(logResult.Result.isSuccessful);
        }
    }
}