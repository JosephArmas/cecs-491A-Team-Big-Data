using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.UserServices;
//using TeamBigData.Utification.UserOfferingsServices;
using TeamBigData.Utification.SQLDataAccess.UsersDB;
using TeamBigData.Utification.SQLDataAccess.LogsDB;
using TeamBigData.Utification.ServiceOfferingsManagers;
using TeamBigData.Utification.SQLDataAccess.LogsDB.Abstractions;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TeamBigData.Utification.ServiceOfferingsServices;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;

namespace TeamBigData.Utification.UserOfferingsTests
{
    [TestClass]
    public class UserServicesIntegrationTests
    {
        private readonly string _featuresConnection;
        private readonly string _usersConnection;
        private readonly string _logConnection;
        private IServiceCollection _services;
        private IServiceProvider _provider;

        public UserServicesIntegrationTests()
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
        Random name = new Random();


        #region Creation


        [TestMethod]
        public async Task Pass_UserEntersServiceAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceOfferingManager>();
            var serv = new ServiceModel();
            serv.ServiceName = name.ToString();
            serv.ServiceDescription = "Er um Erm";
            serv.ServicePhone = "12093436077";
            serv.ServiceURL = "www.website.com";
            serv.Distance = 20;
            serv.ServiceLat = "2.3";
            serv.ServiceLong = "2.5";
            serv.CreatedBy = 1012;
            serv.PinTypes = 3;
            //Act

            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //var result = true;
            //Assert
            //Console.WriteLine(result.data);
            Console.WriteLine(result.ErrorMessage);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccessful);
        }


        [TestMethod]
        public async Task Pass_UserDeletesServiceAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceOfferingManager>();
            var serv = new ServiceModel();
            serv.ServiceName = name.ToString();
            serv.CreatedBy = 1012;
            //Act

            var result = await manager.unregister(serv).ConfigureAwait(false);
            //var result = true;
            //Assert
            //Console.WriteLine(result.data);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccessful);
        }


        [TestMethod]
        public async Task Pass_UserUpdatesServiceAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceOfferingManager>();
            var serv = new ServiceModel();
            serv.ServiceName = name.ToString();
            serv.ServiceDescription = "Er um Erm";
            serv.ServicePhone = "12093445077";
            serv.ServiceURL = "www.new.com";
            serv.Distance = 15;
            serv.ServiceLat = "2.3";
            serv.ServiceLong = "2.5";
            serv.CreatedBy = 1012;
            serv.PinTypes = 2;
            //Act

            var result = await manager.UpdateService(serv).ConfigureAwait(false);
            //var result = true;
            //Assert
            //Console.WriteLine(result.data);
            Console.WriteLine(result.ErrorMessage);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccessful);
        }


        [TestMethod]
        public async Task Fails_UserServiceNotDeletedAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceOfferingManager>();
            var serv = new ServiceModel();
            serv.ServiceName = "WooHoo";
            serv.CreatedBy = 2;
            //Act

            var result = await manager.unregister(serv).ConfigureAwait(false);
            //var result = true;
            //Assert
            //Console.WriteLine(result.data);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
        }


        [TestMethod]
        public async Task Fails_UserServiceNotEnteredAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceOfferingManager>();
            var serv = new ServiceModel();
            serv.ServiceName = "WhoDatBe";
            serv.ServiceDescription = "Er um Erm";
            serv.ServicePhone = "12093436077";
            serv.ServiceURL = "www.website.com";
            serv.Distance = 20;
            serv.ServiceLat = "2.3";
            serv.ServiceLong = "2.5";
            serv.CreatedBy = 2;
            serv.PinTypes = 3;
            //Act

            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //var result = true;
            //Assert
            //Console.WriteLine(result.data);
            Console.WriteLine(result.ErrorMessage);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
        }
        [TestMethod]
        public async Task Fails_UserServiceNotUpdatedAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceOfferingManager>();
            var serv = new ServiceModel();
            serv.ServiceName = "WhoDatBe";
            serv.ServiceDescription = "Er um Erm";
            serv.ServicePhone = "12093436077";
            serv.ServiceURL = "www.website.com";
            serv.Distance = 20;
            serv.ServiceLat = "2.3";
            serv.ServiceLong = "2.5";
            serv.CreatedBy = 2;
            serv.PinTypes = 3;
            //Act

            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //var result = true;
            //Assert
            //Console.WriteLine(result.data);
            Console.WriteLine(result.ErrorMessage);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
        }
        [TestMethod]
        public async Task Fails_UserAccountNotBeingUpdatedToServiceAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceOfferingManager>();
            var serv = new ServiceModel();
            serv.ServiceName = new Random().ToString();
            serv.ServiceDescription = "Er um Erm";
            serv.ServicePhone = "12093436077";
            serv.ServiceURL = "www.website.com";
            serv.Distance = 20;
            serv.ServiceLat = "2.3";
            serv.ServiceLong = "2.5";
            serv.CreatedBy = 2;
            serv.PinTypes = 3;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            //Console.WriteLine(result.data);
            Console.WriteLine(result.ErrorMessage);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
        }
        #endregion
        #region Requests
        //---Requests---
        [TestMethod]
        public async Task Pass_UserEntersRequestAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceRequestManager>();
            var serv = new ServiceModel();
            serv.ServiceName = "WhoDatBe";
            serv.ServiceDescription = "Er um Erm";
            serv.ServicePhone = "12093436077";
            serv.ServiceURL = "www.website.com";
            serv.ServiceID = 8;
            serv.ServiceLat = "2.3";
            serv.ServiceLong = "2.5";
            serv.CreatedBy = 2;
            serv.PinTypes = 3;

            var pin = new Pin();
            pin.UserID = 99;
            pin.Lat = "9.5";
            pin.Lng = "8.5";
            pin.PinType = 3;
            //Act

            /*var result = await manager.ServiceRequest(serv, pin).ConfigureAwait(false);
            //var result = true;
            //Assert
            Console.WriteLine(result.data);
            Console.WriteLine(result.errorMessage);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.isSuccessful);*/
        }


        [TestMethod]
        public async Task Pass_UserAcceptsRequestAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceRequestManager>();
            var req = new RequestModel();
            var serv = new ServiceModel();
            serv.ServiceName = "WhoDatBe";
            serv.ServiceDescription = "Er um Erm";
            serv.ServicePhone = "12093436077";
            serv.ServiceURL = "www.website.com";
            serv.ServiceID = 8;
            serv.ServiceLat = "2.3";
            serv.ServiceLong = "2.5";
            serv.CreatedBy = 2;
            serv.PinTypes = 3;
            //Act

            /*var result = await manager.ProvAcceptRequest(req).ConfigureAwait(false);
            //var result = true;
            //Assert
            Console.WriteLine(result.data);
            Console.WriteLine(result.errorMessage);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.isSuccessful);*/
        }

        [TestMethod]
        public async Task Pass_UserDenysRequestAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceRequestManager>();
            var req = new RequestModel();
            var serv = new ServiceModel();
            serv.ServiceName = "WhoDatBe";
            serv.ServiceDescription = "Er um Erm";
            serv.ServicePhone = "12093436077";
            serv.ServiceURL = "www.website.com";
            serv.ServiceID = 8;
            serv.ServiceLat = "2.3";
            serv.ServiceLong = "2.5";
            serv.CreatedBy = 2;
            serv.PinTypes = 3;
            req.RequestID = 2;


            //Act

            /*var result = await manager.ProvCancelRequest(req).ConfigureAwait(false);


            //Assert

            Console.WriteLine(result.data);
            Console.WriteLine(result.errorMessage);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.isSuccessful);*/
        }

        [TestMethod]
        public async Task Pass_GetsProverRequestsAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceRequestManager>();
            var serv = new ServiceModel();
            serv.ServiceName = "WhoDatBe";
            serv.ServiceID = 8;
            //Act
            /*var result = await manager.GetProviderRequests(serv).ConfigureAwait(false);
            //Assert
            List<RequestModel> resultdata = (List<RequestModel>)result.data;
            Console.WriteLine(resultdata.Count);
            Console.WriteLine(result.errorMessage);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.data.GetType() == typeof(List<RequestModel>));*/
        }
        [TestMethod]
        public async Task Pass_GetsServicesAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceRequestManager>();
            var pin = new Pin();
            var dist = 25;
            //Act
            var result = await manager.getservice(pin, dist).ConfigureAwait(false);
            //Assert
            /*List<ServiceModel> resultdata = (List<ServiceModel>)result.data;
            Console.WriteLine(resultdata.Count);
            Console.WriteLine(result.errorMessage);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.data.GetType() == typeof(List<ServiceModel>));*/
        }
        [TestMethod]
        public async Task Fails_RequestUnableToBeProcessedAsync()
        {
            //Arrange
            var manager = _provider.GetRequiredService<ServiceRequestManager>();

            var serv = new ServiceModel();
            serv.ServiceDescription = "Er um Erm";
            serv.ServicePhone = "12093436077";
            serv.ServiceURL = "www.website.com";
            serv.ServiceID = 8;
            serv.ServiceLat = "2.3";
            serv.ServiceLong = "2.5";
            serv.CreatedBy = 2;
            serv.PinTypes = 3;

            var pin = new Pin();
            pin.UserID = 99;
            pin.Lat = "9.5";
            pin.Lng = "8.5";
            pin.PinType = 3;
            //Act

            /*var result = await manager.ServiceRequest(serv, pin).ConfigureAwait(false);
            //var result = true;
            //Assert
            Console.WriteLine(result.data);
            Console.WriteLine(result.errorMessage);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.isSuccessful);*/
        }
        [TestMethod]
        public async Task Pass_UsersCancelsRequestAsync()
        {

        }

        [TestMethod]
        public async Task Fails_ProviderUnableToCancel3HoursAsync()
        {

        }
        [TestMethod]
        public async Task Fails_RequestorUnableToCancel3HoursAsync()
        {

        }
        [TestMethod]
        public async Task Fails_RequestOutsideRangeAsync()
        {

        }
        [TestMethod]
        public async Task Fails_LimitOf25Async()
        {

        }
        //User is able to cancel a service
        //Provider is unable to cancel a service because they are within 3 hours
        //Requestor is unable to cancel a service because they are within 3 hours
        //Request is outside of ther service range
        //Request accept limit
        #endregion
    }
}
