using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.ServiceOfferingsManagers;
using TeamBigData.Utification.ServiceOfferingsServices;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.UserServices;
using TeamBigData.Utification.SQLDataAccess.LogsDB;
using TeamBigData.Utification.SQLDataAccess.LogsDB.Abstractions;
using TeamBigData.Utification.SQLDataAccess.UsersDB;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;
//using TeamBigData.Utification.UserOfferingsServices;

namespace TeamBigData.Utification.UserOfferingsTests
{
    [TestClass]
    public class UserServicesUnitTests
    {
        private readonly string _featuresConnection;
        private readonly string _usersConnection;
        private readonly string _logConnection;
        private IServiceCollection _services;
        private IServiceProvider _provider;

        public UserServicesUnitTests()
        {
            _featuresConnection = "Server=.\\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False";
            _usersConnection = "Server=.\\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
            _logConnection = "Server=.\\;Database=TeamBigData.Utification.Logs;User=AppUser; Password=t; TrustServerCertificate=True; Encrypt=True";
            _services = new ServiceCollection();
            _services.AddDbContext<IServicesDBInserter, ServicesSqlDAO>(options => options.UseSqlServer(_featuresConnection));
            _services.AddDbContext<IServicesDBSelecter, ServicesSqlDAO>(options => options.UseSqlServer(_featuresConnection));
            _services.AddDbContext<IServicesDBUpdater, ServicesSqlDAO>(options => options.UseSqlServer(_featuresConnection));
            _services.AddDbContext<IUsersDBUpdater, UsersSqlDAO>(options => options.UseSqlServer(_usersConnection));
            // _services.AddDbContext<ServicesSqlDAO>(options => options.UseSqlServer(_featuresConnection));
            //_services.AddDbContext<UsersSqlDAO>(options => options.UseSqlServer(_usersConnection));
            _services.AddDbContext<ILogsDBInserter, LogsSqlDAO>(options => options.UseSqlServer(_logConnection));
            _services.AddTransient<ILogger, Logger>();
            _services.AddTransient<ServiceOfferingService>();
            _services.AddTransient<ServiceRequestService>();
            _services.AddTransient<ServiceOfferingManager>();
            _services.AddTransient<ServiceRequestManager>();
            _provider = _services.BuildServiceProvider();
        }
        #region Fakes
        #endregion
        #region Service Offerings UnitTests
        #region Service Title



        [TestMethod]
        public async Task Fails_UserEntersLongTitleAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceOfferingManager>();
            var serv = new ServiceModel();
            serv.ServiceName = "Lorem ipsum dolor sit amet aliquam.";
            serv.ServiceDescription = "Erm";
            serv.ServicePhone = "15623438888";
            serv.PinTypes = 13;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            Console.WriteLine(result.ErrorMessage);
            Assert.IsNotNull(result);
            //Assert.IsTrue(result.data.ToString() == serv.ServiceName);
            Assert.IsFalse(result.IsSuccessful);
        }

        [TestMethod]
        public async Task Fails_UserEntersShortTitleAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceOfferingManager>();
            var serv = new ServiceModel();
            serv.ServiceName = "Lor";
            serv.ServiceDescription = "Erm";
            serv.ServicePhone = "15623438888";
            serv.PinTypes = 13;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            Console.WriteLine(result.ErrorMessage);
            Assert.IsNotNull(result);
            //Assert.IsTrue(result.data == serv.ServiceName);
            Assert.IsFalse(result.IsSuccessful);
        }

        [TestMethod]
        public async Task Fails_UserEntersInvalidCharAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceOfferingManager>();
            var serv = new ServiceModel();
            serv.ServiceName = "Lor,";
            serv.ServiceDescription = "Erm";
            serv.ServicePhone = "15623438888";
            serv.PinTypes = 13;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            Console.WriteLine(result.ErrorMessage);
            Assert.IsNotNull(result);
            //Assert.IsTrue((string)result.data == serv.ServiceName);
            Assert.IsFalse(result.IsSuccessful);
        }
        #endregion

        #region Service Description
        /// <summary>
        /// Test showing the invalid character for the description
        /// </summary>
        [TestMethod]
        public async Task Fails_UserEntersInvalidCharDescAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceOfferingManager>();
            var serv = new ServiceModel();
            serv.ServiceName = "Lorem ipsum dolor sit amet aliquam.";
            serv.ServiceDescription = ",";
            serv.ServicePhone = "15623438888";
            serv.PinTypes = 13;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            Console.WriteLine(result.ErrorMessage);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
        }
        /// <summary>
        /// The character limit 1001
        /// </summary>
        [TestMethod]
        public async Task Fails_UserEntersLongDescCharacterLimitAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceOfferingManager>();
            var serv = new ServiceModel();
            serv.ServiceName = "Lorem ipsum dolor sit amet aliquam.";
            serv.ServiceDescription = "ggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg";
            serv.ServicePhone = "15623438888";
            serv.PinTypes = 13;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            Console.WriteLine(result.ErrorMessage);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
        }
        /// <summary>
        /// The "Word" limit is 150 but what defines a word? For this test it is the number of spaces
        /// </summary>
        [TestMethod]
        public async Task Fails_UserEntersLongDescWordLimitAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceOfferingManager>();
            var serv = new ServiceModel();
            serv.ServiceName = "Lorem ipsum dolor sit amet aliquam.";
            serv.ServiceDescription = "                                                                                                                                                       ";
            serv.ServicePhone = "15623438888";
            serv.PinTypes = 13;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            Console.WriteLine(result.ErrorMessage);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
        }

        #endregion

        #region Service Phone
        [TestMethod]
        public async Task Fails_UserEntersWrongPhoneAreaCodeAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceOfferingManager>();
            var serv = new ServiceModel();
            serv.ServiceName = "Lorem ipsum dolor sit amet aliquam.";
            serv.ServiceDescription = "Erm";
            serv.ServicePhone = "15633438888";
            serv.PinTypes = 13;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            Console.WriteLine(result.ErrorMessage);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
        }
        [TestMethod]
        public async Task Fails_UserEntersWrongPhoneCountryCodeAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceOfferingManager>();
            var serv = new ServiceModel();
            serv.ServiceName = "Lorem ipsum dolor sit amet aliquam.";
            serv.ServiceDescription = "Erm";
            serv.ServicePhone = "25623438888";
            serv.PinTypes = 13;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            Console.WriteLine(result.ErrorMessage);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
        }
        [TestMethod]
        public async Task Fails_UserEntersWrongPhoneLengthAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceOfferingManager>();
            var serv = new ServiceModel();
            serv.ServiceName = "Lorem ipsum dolor sit amet aliquam.";
            serv.ServiceDescription = "Erm";
            serv.ServicePhone = "1562334434388455655444444446888";
            serv.PinTypes = 13;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            Console.WriteLine(result.ErrorMessage);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
        }

        #endregion

        #region Pins
        [TestMethod]
        public async Task Fails_UserSelectsInvalidPinsAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceOfferingManager>();
            var serv = new ServiceModel();
            serv.ServiceName = "Lorem ipsum dolor sit amet aliquam.";
            serv.ServiceDescription = "Erm";
            serv.ServicePhone = "15623344343888";
            serv.PinTypes = 136;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            Console.WriteLine(result.ErrorMessage);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
        }
        #endregion
        #endregion

        #region Service Requests UnitTests
        [TestMethod]
        public async Task Fails_UserSelectsInvalidDistanceAsync()
        {
            //Arrange

            var manager = _provider.GetRequiredService<ServiceRequestManager>();
            var serv = new ServiceModel();
            var pin = new Pin();
            pin.Lat = "3.5";
            int dist = 26;

            //Act

            var result = await manager.getservice(pin, dist).ConfigureAwait(false);

            //Assert

            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ErrorMessage == "Distance is outside of permitted area");
        }
        #endregion
    }
}