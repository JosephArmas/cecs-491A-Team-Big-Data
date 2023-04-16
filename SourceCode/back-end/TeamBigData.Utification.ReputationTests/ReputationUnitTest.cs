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
            Report report = new Report(5.0, 1001, 1002, "This is to test the role changing works");
            UserAccount userAccount = new UserAccount(1001, "CatchesDuplicateEmailTest@yahoo.com", "", "", "8A-C2-0D-FD-44-9A-72-DB-86-C6-C2-6D-98-A5-2E-E9-58-3E-20-D4-A0-26-D8-63-70-F0-4E-07-C6-C8-C9-E1-25-2D-2E-2F-49-CC-DE-F1-C7-8E-F2-3B-64-84-EE-97-6F-72-DF-0F-45-B8-EC-6E-4D-E7-47-04-2A-77-88-A5");
            UserProfile userProfile = new UserProfile();
            Logger logger = new Logger(new SqlDAO( @"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            ReputationService repSer = new ReputationService(result, reportsDao, reportsDao, profileDAO, profileDAO, report, userAccount, userProfile, logger);
            ReputationManager repMan = new ReputationManager(repSer, result, report, logger, userAccount, userProfile);

            //Act
            var act = repMan.RecordNewUserReport(4.2);
            
            //Assert
            Assert.IsTrue(act.Result.isSuccessful);
        }
    }
}