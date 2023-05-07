using TeamBigData.Utification.FileManagers;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB;
using TeamBigData.Utification.FileServices;

namespace TeamBigData.Utification.FileTests
{
    /*
    [TestClass]
    public class FileIntegrationTests
    {
        private readonly String fileConnection;
        private readonly String logConnection;
        public FileIntegrationTests()
        {
            fileConnection = "Server=.\\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False";
            logConnection = "Server=.\\;Database=TeamBigData.Utification.Logs;User=AppUser; Password=t; TrustServerCertificate=True; Encrypt=True";
        }

        [TestMethod]
        public async Task UploadFileToPin()
        {
            //Arrange
            var fileDao = new FileSqlDAO(fileConnection);
            var pinDao = new PinsSqlDAO(fileConnection);
            var service = new FileService(fileDao);
            var manager = new FileManager(service, fileDao, pinDao);
            var admin = new UserProfile(1, "Admin User");
            //Act
            var response1 = await manager.DeletePinPic(1, admin);
            var response2 = await manager.UploadPinPic("tes.jpg", 1, admin);
            var data = (String)response2.Data;
            //Assert
            Assert.IsTrue(response2.IsSuccessful);
            Assert.IsTrue(data.Length > 10);
        }

        [TestMethod]
        public async Task UploadFileToProfile()
        {
            //Arrange
            var fileDao = new FileSqlDAO(fileConnection);
            var pinDao = new PinsSqlDAO(fileConnection);
            var service = new FileService(fileDao);
            var manager = new FileManager(service, fileDao, pinDao);
            var admin = new UserProfile(1, "Admin User");
            //Act
            var response1 = await manager.DeleteProfilePic(1, admin);
            var response2 = await manager.UploadProfilePic("tes.jpg", 1, admin);
            var data = (String)response2.Data;
            //Assert
            Assert.IsTrue(response2.IsSuccessful);
            Assert.IsTrue(data.Length > 10);
        }

    }*/
}