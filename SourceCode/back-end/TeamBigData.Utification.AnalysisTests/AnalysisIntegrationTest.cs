using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.AnalysisManagers;
using TeamBigData.Utification.Cryptography;

namespace TeamBigData.Utification.AnalysisTests
{
    [TestClass]
    public class AnalysisIntegrationTest
    {
        [TestMethod]
        public async Task ChecksTodaysLogins()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True";
            var loginSqlDAO = new SqlDAO(connectionString);
            var manager = new AnalysisManager();
            var insertSQl = @"Insert into dbo.logs values ('1', 'Info', '', GetDate(), 'SecurityManager.LoginUser()', 'Testing', 'Sucessful Login')";
            //Act
            var before = manager.GetLogins().Result;
            var response = await loginSqlDAO.Execute(insertSQl);
            var after = manager.GetLogins().Result;
            //Assert
            Assert.IsTrue(response.isSuccessful);
            Assert.AreEqual(before.data[90] + 1, after.data[90]);
        }

        [TestMethod]
        public async Task ChecksFurthestLogins()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True";
            var loginSqlDAO = new SqlDAO(connectionString);
            var manager = new AnalysisManager();
            var insertSQl = @"Insert into dbo.logs values ('1', 'Info', '', DateAdd(Hour, -24*90 + 1, GetDate()), 'SecurityManager.LoginUser()', 'Testing', 'Sucessful Login')";
            //Act
            var before = await manager.GetLogins();
            var response = await loginSqlDAO.Execute(insertSQl);
            var after = await manager.GetLogins();
            //Assert
            Assert.IsTrue(response.isSuccessful);
            Assert.AreEqual(before.data[0] + 1, after.data[0]);
        }
    }
}