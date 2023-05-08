using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.AnalysisManagers;
using TeamBigData.Utification.Cryptography;
using System.Data.Common;
using TeamBigData.Utification.SQLDataAccess.LogsDB;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.AnalysisTests
{
    /*[TestClass]
    public class AnalysisIntegrationTest
    {
        private readonly String connectionString;
        public AnalysisIntegrationTest()
        {
            connectionString = @"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True";
        }

        [TestMethod]
        public async Task ChecksTodaysLogins()
        {
            //Arrange
            var SqlDAO = new SqlDAO(connectionString);
            var manager = new AnalysisManager(SqlDAO);
            var insertSQl = @"Insert into dbo.logs values ('1', 'Info', '', GetDate(), 'SecurityManager.LoginUser()', 'Testing', 'Sucessful Login')";
            //Act
            var before = manager.GetLogins().Result;
            var response = await SqlDAO.Execute(insertSQl);
            var after = manager.GetLogins().Result;
            //Assert
            Assert.IsTrue(response.IsSuccessful);
            Assert.AreEqual(before.Data[90] + 1, after.Data[90]);
        }

        [TestMethod]
        public async Task ChecksFurthestLogins()
        {
            //Arrange
            var loginSqlDAO = new SqlDAO(connectionString);
            var manager = new AnalysisManager(loginSqlDAO);
            var insertSQl = @"Insert into dbo.logs values ('1', 'Info', '', DateAdd(Hour, -24*90 + 1, GetDate()), 'SecurityManager.LoginUser()', 'Testing', 'Sucessful Login')";
            //Act
            var before = await manager.GetLogins();
            var response = await loginSqlDAO.Execute(insertSQl);
            var after = await manager.GetLogins();
            //Assert
            Assert.IsTrue(response.IsSuccessful);
            Assert.AreEqual(before.Data[0] + 1, after.Data[0]);
        }
    }*/
}