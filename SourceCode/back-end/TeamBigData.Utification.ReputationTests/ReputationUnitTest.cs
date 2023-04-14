using TeamBigData.Utification.ReputationServices;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Manager;

namespace TeamBigData.Utification.ReputationTests
{
    [TestClass]
    public class ReputationUnitTest
    {
        [TestMethod]
        public void SubmitReportAndAffectReputation()
        {
            //Arrange
            Response result = new Response();
            SqlDAO reportsDao = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Features;User=AppUser;Password=t;TrustServerCertificate=True;Integrated Security=False;Encrypt=True");
            SqlDAO userDAO = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Users;User=AppUser;Password=t;TrustServerCertificate=True;Integrated Security=False");
            SqlDAO profileDAO = new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Users;User=AppUser;Password=t;TrustServerCertificate=True;Integrated Security=False");
            Report report = new Report(3.5, 1005, 1006, "This is to test retrieving reports");
            UserAccount userAccount = new UserAccount(1005, "CreateLogWhenRegisteringTesttCgjhg==@yahoo.com", "", "", "");
            UserProfile userProfile = new UserProfile();
            Logger logger = new Logger(new SqlDAO( @"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            ReputationService repSer = new ReputationService(result, reportsDao, reportsDao, profileDAO, profileDAO, report, userAccount, userProfile);
            ReputationManager repMan = new ReputationManager(repSer, result, logger, userAccount);

            //Act
            var act = repMan.RecordNewUserReport();
            
            //Assert
            Assert.IsTrue(act.Result.isSuccessful);
        }
    }
}