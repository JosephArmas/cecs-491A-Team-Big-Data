using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.UserServicesManager;
using TeamBigData.Utification.UserServicesManager.Interfaces;

namespace TeamBigData.Utification.UserServicesTests
{
    [TestClass]
    public class UserServicesUnitTests
    {
        #region Fakes
        public class FakeManager
        {
            public Task<Response> CreateService(ServModel Serv)
            {
                throw new NotImplementedException();
            }

            public Task<Response> unregister(ServModel Serv)
            {
                throw new NotImplementedException();
            }

            public Task<Response> UpdateService(ServModel Serv)
            {
                throw new NotImplementedException();
            }
        }
        #endregion
        #region ServiceProvider UnitTests
        #region Service Title


        [TestMethod]
        public async Task Fails_UserEntersLongTitleAsync()
        {
            //Arrange
            var manager = new ServiceProviderManager();
            var serv = new ServModel();
            serv.ServiceName = "Lorem ipsum dolor sit amet aliquam.";
            serv.ServiceDescription = "Erm";
            serv.ServicePhone = "15623438888";
            serv.PinTypes = 13;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            Console.WriteLine(result.errorMessage);
            Assert.IsNotNull(result);
            Assert.IsTrue((result.data.ToString() == serv.ServiceName));
            Assert.IsFalse(result.isSuccessful);
        }

        [TestMethod]
        public async Task Fails_UserEntersShortTitleAsync()
        {
            //Arrange
            var manager = new ServiceProviderManager();
            var serv = new ServModel();
            serv.ServiceName = "Lor";
            serv.ServiceDescription = "Erm";
            serv.ServicePhone = "15623438888";
            serv.PinTypes = 13;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            Console.WriteLine(result.errorMessage);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.data == serv.ServiceName);
            Assert.IsFalse(result.isSuccessful);
        }

        [TestMethod]
        public async Task Fails_UserEntersInvalidCharAsync()
        {
            //Arrange
            var manager = new ServiceProviderManager();
            var serv = new ServModel();
            serv.ServiceName = "Lor,";
            serv.ServiceDescription = "Erm";
            serv.ServicePhone = "15623438888";
            serv.PinTypes = 13;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            Console.WriteLine(result.errorMessage);
            Assert.IsNotNull(result);
            Assert.IsTrue((String)result.data == serv.ServiceName);
            Assert.IsFalse(result.isSuccessful);
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
            var manager = new ServiceProviderManager();
            var serv = new ServModel();
            serv.ServiceName = "Lorem ipsum dolor sit amet aliquam.";
            serv.ServiceDescription = ",";
            serv.ServicePhone = "15623438888";
            serv.PinTypes = 13;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            Console.WriteLine(result.errorMessage);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.isSuccessful);
        }
        /// <summary>
        /// The character limit 1001
        /// </summary>
        [TestMethod]
        public async Task Fails_UserEntersLongDescCharacterLimitAsync()
        {
            //Arrange
            var manager = new ServiceProviderManager();
            var serv = new ServModel();
            serv.ServiceName = "Lorem ipsum dolor sit amet aliquam.";
            serv.ServiceDescription = "ggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg";
            serv.ServicePhone = "15623438888";
            serv.PinTypes = 13;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            Console.WriteLine(result.errorMessage);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.isSuccessful);
        }
        /// <summary>
        /// The "Word" limit is 150 but what defines a word? For this test it is the number of spaces
        /// </summary>
        [TestMethod]
        public async Task Fails_UserEntersLongDescWordLimitAsync()
        {
            //Arrange
            var manager = new ServiceProviderManager();
            var serv = new ServModel();
            serv.ServiceName = "Lorem ipsum dolor sit amet aliquam.";
            serv.ServiceDescription = "                                                                                                                                                       ";
            serv.ServicePhone = "15623438888";
            serv.PinTypes = 13;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            Console.WriteLine(result.errorMessage);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.isSuccessful);
        }

        #endregion

        #region Service Phone
        [TestMethod]
        public async Task Fails_UserEntersWrongPhoneAreaCodeAsync()
        {
            //Arrange
            var manager = new ServiceProviderManager();
            var serv = new ServModel();
            serv.ServiceName = "Lorem ipsum dolor sit amet aliquam.";
            serv.ServiceDescription = "Erm";
            serv.ServicePhone = "15633438888";
            serv.PinTypes = 13;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            Console.WriteLine(result.errorMessage);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.isSuccessful);
        }
        [TestMethod]
        public async Task Fails_UserEntersWrongPhoneCountryCodeAsync()
        {
            //Arrange
            var manager = new ServiceProviderManager();
            var serv = new ServModel();
            serv.ServiceName = "Lorem ipsum dolor sit amet aliquam.";
            serv.ServiceDescription = "Erm";
            serv.ServicePhone = "25623438888";
            serv.PinTypes = 13;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            Console.WriteLine(result.errorMessage);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.isSuccessful);
        }
        [TestMethod]
        public async Task Fails_UserEntersWrongPhoneLengthAsync()
        {
            //Arrange
            var manager = new ServiceProviderManager();
            var serv = new ServModel();
            serv.ServiceName = "Lorem ipsum dolor sit amet aliquam.";
            serv.ServiceDescription = "Erm";
            serv.ServicePhone = "1562334434388455655444444446888";
            serv.PinTypes = 13;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            Console.WriteLine(result.errorMessage);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.isSuccessful);
        }

        #endregion
        [TestMethod]
        public async Task Fails_UserSelectsInvalidPinsAsync()
        {
            //Arrange
            var manager = new ServiceProviderManager();
            var serv = new ServModel();
            serv.ServiceName = "Lorem ipsum dolor sit amet aliquam.";
            serv.ServiceDescription = "Erm";
            serv.ServicePhone = "15623344343888";
            serv.PinTypes = 136;
            //Act
            var result = await manager.CreateService(serv).ConfigureAwait(false);
            //Assert
            Console.WriteLine(result.errorMessage);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.isSuccessful);
        }
        #endregion
    }
}