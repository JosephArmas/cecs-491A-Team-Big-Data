using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.UserServicesManager;

namespace TeamBigData.Utification.UserServicesTests
{
    [TestClass]
    public class UserServicesIntegrationTests
    {
        string name = new Random().Next().ToString();
        #region Creation
        [TestMethod]
        public async Task Pass_UserEntersServiceAsync()
        {
            //Arrange
            var manager = new ServiceProviderManager();
            var serv = new ServModel();
            serv.ServiceName = name;
            serv.ServiceDescription = "Er um Erm";
            serv.ServicePhone = "12093436077";
            serv.ServiceURL = "www.website.com";
            serv.Distance = 20;
            serv.ServiceLat = "2.3";
            serv.ServiceLong = "2.5";
            serv.CreatedBy = 1002;
            serv.PinTypes = 3;
            //Act

            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //var result = true;
            //Assert
            Console.WriteLine(result.data);
            Console.WriteLine(result.errorMessage);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.isSuccessful);
        }
        [TestMethod]
        public async Task Pass_UserDeletesServiceAsync()
        {
            //Arrange
            var manager = new ServiceProviderManager();
            var serv = new ServModel();
            serv.ServiceName = name;
            serv.CreatedBy = 1002;
            //Act

            var result = await manager.unregister(serv).ConfigureAwait(false);
            //var result = true;
            //Assert
            Console.WriteLine(result.data);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.isSuccessful);
        }
        [TestMethod]
        public async Task Pass_UserUpdatesServiceAsync()
        {
            //Arrange
            var manager = new ServiceProviderManager();
            var serv = new ServModel();
            serv.ServiceName = name;
            serv.ServiceDescription = "Er um Erm";
            serv.ServicePhone = "12093445077";
            serv.ServiceURL = "www.new.com";
            serv.Distance = 15;
            serv.ServiceLat = "2.3";
            serv.ServiceLong = "2.5";
            serv.CreatedBy = 1002;
            serv.PinTypes = 2;
            //Act

            var result = await manager.UpdateService(serv).ConfigureAwait(false);
            //var result = true;
            //Assert
            Console.WriteLine(result.data);
            Console.WriteLine(result.errorMessage);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.isSuccessful);
        }
        [TestMethod]
        public async Task Fails_UserServiceNotEnteredAsync()
        {
            //Arrange
            var manager = new ServiceProviderManager();
            var serv = new ServModel();
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
            Console.WriteLine(result.data);
            Console.WriteLine(result.errorMessage);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.isSuccessful);
        }
#endregion
        #region Requests
        //---Requests---
        [TestMethod]
        public async Task Pass_UserEntersRequestAsync()
        {
            //Arrange
            var manager = new ServRequestManager();

            var serv = new ServModel();
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
            pin._userID = 99;
            pin._lat = "9.5";
            pin._lng = "8.5";
            pin._pinType = 3;
            //Act

            var result = await manager.ServiceRequest(serv,pin).ConfigureAwait(false);
            //var result = true;
            //Assert
            Console.WriteLine(result.data);
            Console.WriteLine(result.errorMessage);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.isSuccessful);
        }
        [TestMethod]
        public async Task Pass_UserDenysRequestAsync()
        {
            //Arrange
            var manager = new ServRequestManager();
            var req = new RequestModel();
            var serv = new ServModel();
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

            var result = await manager.ProvCancelRequest(req).ConfigureAwait(false);
            //var result = true;
            //Assert
            Console.WriteLine(result.data);
            Console.WriteLine(result.errorMessage);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.isSuccessful);
        }
        [TestMethod]
        public async Task Pass_GetsProverRequestsAsync()
        {
            //Arrange
            var dao = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False");
            var manager = new ServRequestManager();
            var serv = new ServModel();
            serv.ServiceName = "WhoDatBe";
            serv.ServiceID = 8;
            //Act
            var result = await manager.GetProviderRequests(serv).ConfigureAwait(false);
            //Assert
            //List<RequestModel> resultdata = (List<RequestModel>)result.data;
            //Console.WriteLine(resultdata.Count);
            Console.WriteLine(result.errorMessage);
            Assert.IsNotNull(result);
            //Assert.IsTrue(result.data.GetType() == typeof(List<RequestModel>));
        }
        [TestMethod]
        public async Task Pass_GetsServicesAsync()
        {
            //Arrange
            var dao = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False");
            var manager = new ServRequestManager();
            var pin = new Pin();
            var dist = 25;
            //Act
            var result = await manager.getservice(pin,dist).ConfigureAwait(false);
            //Assert
            List<ServModel> resultdata = (List<ServModel>)result.data;
            Console.WriteLine(resultdata.Count);
            Console.WriteLine(result.errorMessage);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.data.GetType() == typeof(List<ServModel>));
        }
        #endregion
       
    }
}
