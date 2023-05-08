using TeamBigData.Utification.PinManagers;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.PinServices;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.LogsDB;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Models;
using Microsoft.Extensions.DependencyInjection;
using TeamBigData.Utification.Logging.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace TeamBigData.Utification.PinTests
{
    /*[TestClass]
    public class PinUnitTest
    {
        private IServiceCollection _services;
        private IServiceProvider _provider;

        public PinUnitTest()
        {
            // Arrange
            _services = new ServiceCollection();

            _services.AddDbContext<LogsSqlDAO>(options => options.UseSqlServer("Server=.\\;Database=TeamBigData.Utification.Logs;User=AppUser; Password=t; TrustServerCertificate=True; Encrypt=True"));
            _services.AddTransient<ILogger, Logger>();

            //_services.AddDbContext<SqlDAO>(options => options.UseSqlServer("Server=.\\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False"));
            _services.AddDbContext<PinsSqlDAO>(options => options.UseSqlServer("Server=.\\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False"));
            //_services.AddDbContext<IPinDBInserter, PinsSqlDAO>();//options => options.UseSqlServer("Server=.\\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False"));
            //_services.AddDbContext<IPinDBSelecter, PinsSqlDAO>();//options => options.UseSqlServer("Server=.\\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False"));
            //_services.AddDbContext<IPinDBUpdater, PinsSqlDAO>();//options => options.UseSqlServer("Server=.\\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False"));
            _services.AddTransient<PinService>();
            _services.AddTransient<PinManager>();

            _provider = _services.BuildServiceProvider();
        }

        [TestMethod]
        public async Task PinDependenciesProperlyInjected()
        {
            // Assert
            var pinManager = _provider.GetRequiredService<PinManager>();
            var pinService = _provider.GetRequiredService<PinService>();
            var pinSqlDAO = _provider.GetRequiredService<PinsSqlDAO>();
            var logsSqlDAO = _provider.GetRequiredService<LogsSqlDAO>();
            var logger = _provider.GetRequiredService<ILogger>();

            // Act
            Assert.IsNotNull(logger);
            Assert.IsNotNull(logsSqlDAO);
            Assert.IsNotNull(pinSqlDAO);
            Assert.IsNotNull(pinService);
            Assert.IsNotNull(pinManager);
        }

        
        [TestMethod]
        public async Task GetAllPinsIsLogged()
        {
            //Arrange
            var sysUnderTest = _provider.GetRequiredService<PinManager>();
            UserAccount userAccount = new UserAccount(1001, "test@gmail.com", "66-98-9D-85-60-0D-0B-F1-D7-9D-64-77-2F-6B-8A-BE-79-0A-B1-0E-A9-4C-63-1D-EB-F8-DC-B0-1B-2B-E4-9E-81-28-EE-FF-12-4A-50-E2-30-F8-0B-58-7E-F9-58-04-64-96-35-36-60-FE-2B-FA-BA-CE-69-5E-ED-DD-A6-18", "COkehvygG2520lB+IB3hLkl9e1URsoFa", "C2-15-52-1A-57-17-8D-19-B1-BB-27-C0-71-C2-74-82-2F-9C-2F-2A-5E-D8-CA-83-31-20-D1-7B-8D-DA-BC-E6-84-65-40-BF-71-E8-CB-FF-E0-42-22-82-3D-71-47-C9-61-65-C8-AC-87-0D-63-18-38-B9-86-D5-EA-19-84-32", false);
            //Act
            var result = await sysUnderTest.GetListOfAllEnabledPins("Get All Pins Is Logged Test").ConfigureAwait(false);
            //Assert
            Assert.IsTrue(result.Data.Count > 0);
        }
        [TestMethod]
        public async Task PostNewPinIsLogged()
        {
            //Arrange
            var sysUnderTest = _provider.GetRequiredService<PinManager>();
            //UserAccount userAccount = new UserAccount(1001, "test@gmail.com", "66-98-9D-85-60-0D-0B-F1-D7-9D-64-77-2F-6B-8A-BE-79-0A-B1-0E-A9-4C-63-1D-EB-F8-DC-B0-1B-2B-E4-9E-81-28-EE-FF-12-4A-50-E2-30-F8-0B-58-7E-F9-58-04-64-96-35-36-60-FE-2B-FA-BA-CE-69-5E-ED-DD-A6-18", "COkehvygG2520lB+IB3hLkl9e1URsoFa", "C2-15-52-1A-57-17-8D-19-B1-BB-27-C0-71-C2-74-82-2F-9C-2F-2A-5E-D8-CA-83-31-20-D1-7B-8D-DA-BC-E6-84-65-40-BF-71-E8-CB-FF-E0-42-22-82-3D-71-47-C9-61-65-C8-AC-87-0D-63-18-38-B9-86-D5-EA-19-84-32", false);
            Pin pin = new Pin(1001,"33.797299272696115", "-118.11066677246092", 2, "`<h1>test junk</h1><p></p>`");
            //Act
            var result = await sysUnderTest.SaveNewPin(pin, "Post New Pin Is Logged Test").ConfigureAwait(false);
            //Assert
            Assert.IsTrue(result.IsSuccessful);
        }
    }*/
}