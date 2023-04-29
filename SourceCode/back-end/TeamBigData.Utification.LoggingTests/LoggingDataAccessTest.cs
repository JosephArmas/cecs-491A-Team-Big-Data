using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Models;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using TeamBigData.Utification.SQLDataAccess.LogsDB;
using TeamBigData.Utification.Logging.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace TeamBigData.Utification.LoggingTest
{
    [TestClass]
    public class DataAccessTest
    {
        private IServiceCollection _services;
        private IServiceProvider _provider;
        public DataAccessTest()
        {
            _services = new ServiceCollection();

            _services.AddDbContext<LogsSqlDAO>(options => options.UseSqlServer("Server=.\\;Database=TeamBigData.Utification.Logs;User=AppUser; Password=t; TrustServerCertificate=True; Encrypt=True"));
            _services.AddTransient<ILogger, Logger>();

            _provider = _services.BuildServiceProvider();
        }

        [TestMethod]
        public void DAO_LogMustSaveToDataStore() //If updating the data store make sure to assert each individual column for maximum verification
        {
            //Arrange
            var sysUnderTest = _provider.GetRequiredService<ILogger>();
            var log = new Log(1, "Info", "SYSTEM", "DAO_LogMustSaveToDataStore", "Data", "This is a automated test");
            //Act
            var rows = sysUnderTest.Logs(log).Result;
            //Assert
            Assert.IsTrue(rows.isSuccessful);
        }
        [TestMethod]
        public void DAO_LogMustBeImmutable()
        {
            //Arrange
            var sysUnderTest = _provider.GetRequiredService<LogsSqlDAO>();
            var updateSql = "UPDATE dbo.Logs SET Message = @m WHERE LogID = 5";
            var check = "Log has been updated"; //A check value used to determine if the Command successfully recieves an error.
            //Act
            var rows = sysUnderTest.UpdateLogsTest(updateSql).Result;
            //Assert
            Assert.AreEqual(check, rows.errorMessage);
            Assert.IsFalse(rows.isSuccessful);
        }
        [TestMethod]
        public void DAO_MustLogWithin5Secs()
        {
            //Arrange
            var stopwatch = new Stopwatch();
            var expected = 5000;
            var sysUnderTest = _provider.GetRequiredService<ILogger>();
            var log = new Log(2, "Info", "SYSTEM", "DAO_MustLogWithin5Secs", "Business", "This is a automated test for finding if it took longer than 5 seconds");
            //Act
            stopwatch.Start();
            var logResult = sysUnderTest.Logs(log).Result;
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;
            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(logResult.isSuccessful);
        }
    }
}