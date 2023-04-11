using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.AnalysisManagers;
using Microsoft.AspNetCore.Http.Connections;

namespace TeamBigData.Utification.AnalysisTests
{
    [TestClass]
    public class AnalysisIntegrationTest
    {
        [TestMethod]
        public void ChecksTodaysLogins()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True";
            var loginSqlDAO = new SqlDAO(connectionString);
            var manager = new AnalysisManager();
            var insertSQl = @"Insert into dbo.logs values ('1', 'Info', '', GetDate(), 'SecurityManager.LoginUser()', 'Testing', 'Sucessful Login')";
            //Act
            var before = manager.GetLogins().Result;
            var response = loginSqlDAO.Execute(insertSQl).Result;
            var after = manager.GetLogins().Result;
            //Assert
            Assert.IsTrue(response.isSuccessful);
            Assert.AreEqual(before[90] + 1, after[90]);
        }

        [TestMethod]
        public void ChecksFurthestLogins()
        {
            //Arrange
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True";
            var loginSqlDAO = new SqlDAO(connectionString);
            var manager = new AnalysisManager();
            var insertSQl = @"Insert into dbo.logs values ('1', 'Info', '', DateAdd(Hour, -24*90 + 1, GetDate()), 'SecurityManager.LoginUser()', 'Testing', 'Sucessful Login')";
            //Act
            var before = manager.GetLogins().Result;
            var response = loginSqlDAO.Execute(insertSQl).Result;
            var after = manager.GetLogins().Result;
            //Assert
            Assert.IsTrue(response.isSuccessful);
            Assert.AreEqual(before[0] + 1, after[0]);
        }
    }
}