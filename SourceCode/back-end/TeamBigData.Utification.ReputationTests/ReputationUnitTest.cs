using TeamBigData.Utification.ReputationServices;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.Logging;

namespace TeamBigData.Utification.ReputationTests
{
    [TestClass]
    public class ReputationUnitTest
    {
        [TestMethod]
        public void InsertNewReportToSqlServer()
        {
            //Arrange
            Response result = new Response();
            SqlDAO reportsDao = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Features;User=AppUser;Password=t;TrustServerCertificate=True;Integrated Security=False;Encrypt=True");
            SqlDAO userDAO = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Users;User=AppUser;Password=t;TrustServerCertificate=True;Integrated Security=False");
            Report report = new Report(3.0, 1001, 1002, "This is simply a test");
            UserAccount userAccount = new UserAccount(1049, "testUser@yahoo.com", "", "", "");
            UserProfile userProfile = new UserProfile(1049, 2.0);
            Logger logger = new Logger(new SqlDAO( @"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            ReputationService repSer = new ReputationService(result, reportsDao, userDAO, report, userAccount, userProfile, logger);
            
            //Act
            var act = repSer.UpdateUserReputation();
            
            //Assert
            Assert.IsTrue(act.Result.isSuccessful);
        }
    }
}