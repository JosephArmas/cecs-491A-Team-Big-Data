using TeamBigData.Utification.ReputationServices;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Reports;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;
using TeamBigData.Utification.SQLDataAccess.UsersDB;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.SQLDataAccess.LogsDB;

namespace TeamBigData.Utification.ReputationTests
{
    [TestClass]
    public class ReputationIntegrationTest
    {
        [TestMethod]
        public void SubmitReportAndAffectReputation()
        {
            // Arrange
            IReportsDBInserter insertReport = new ReportsSqlDAO(@"Server=.\;Database=TeamBigData.Utification.Features;User=AppUser;Password=t;TrustServerCertificate=True");
            IReportsDBSelecter selectReport = new ReportsSqlDAO(@"Server=.\;Database=TeamBigData.Utification.Features;User=AppUser;Password=t;TrustServerCertificate=True");
            IUsersDBUpdater updateProfile = new UsersSqlDAO(@"Server=.\;Database=TeamBigData.Utification.Users;User=AppUser;Password=t;TrustServerCertificate=True");
            IUsersDBSelecter selectProfile = new UsersSqlDAO(@"Server=.\;Database=TeamBigData.Utification.Users;User=AppUser;Password=t;TrustServerCertificate=True");
            ILogger logger = new Logger(new LogsSqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            ReputationService repSer = new ReputationService(insertReport, selectReport, updateProfile, selectProfile, logger);
            ReputationManager repMan = new ReputationManager(repSer, logger);
            // The report's user IDs vary upon the device and the users that are in the DB
            Report report = new Report(0.5, 1010, 1011, "This user sucks.");

            // Act
            var act = repMan.RecordNewUserReportAsync(report, 4.2);

            Console.WriteLine(act.Result.ErrorMessage);
            // Assert
            Assert.IsTrue(act.Result.IsSuccessful);
        }

        [TestMethod]
        public void GetReports()
        {
            // Arrange
            IReportsDBInserter insertReport = new ReportsSqlDAO(@"Server=.\;Database=TeamBigData.Utification.Features;User=AppUser;Password=t;TrustServerCertificate=True");
            IReportsDBSelecter selectReport = new ReportsSqlDAO(@"Server=.\;Database=TeamBigData.Utification.Features;User=AppUser;Password=t;TrustServerCertificate=True");
            IUsersDBUpdater updateProfile = new UsersSqlDAO(@"Server=.\;Database=TeamBigData.Utification.Users;User=AppUser;Password=t;TrustServerCertificate=True");
            IUsersDBSelecter selectProfile = new UsersSqlDAO(@"Server=.\;Database=TeamBigData.Utification.Users;User=AppUser;Password=t;TrustServerCertificate=True");
            ILogger logger = new Logger(new LogsSqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            ReputationService repSer = new ReputationService(insertReport, selectReport, updateProfile, selectProfile, logger);
            ReputationManager repMan = new ReputationManager(repSer, logger);
            
            // Act
            var getReports = repSer.GetUserReportsAsync(1001, "");
              

            // Assert
            Assert.IsTrue(getReports.Result.isSuccessful);
        }

        [TestMethod]
        public void GetReputation()
        {
            // Arrange
            IReportsDBInserter insertReport = new ReportsSqlDAO(@"Server=.\;Database=TeamBigData.Utification.Features;User=AppUser;Password=t;TrustServerCertificate=True");
            IReportsDBSelecter selectReport = new ReportsSqlDAO(@"Server=.\;Database=TeamBigData.Utification.Features;User=AppUser;Password=t;TrustServerCertificate=True");
            IUsersDBUpdater updateProfile = new UsersSqlDAO(@"Server=.\;Database=TeamBigData.Utification.Users;User=AppUser;Password=t;TrustServerCertificate=True");
            IUsersDBSelecter selectProfile = new UsersSqlDAO(@"Server=.\;Database=TeamBigData.Utification.Users;User=AppUser;Password=t;TrustServerCertificate=True");
            ILogger logger = new Logger(new LogsSqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            ReputationService repSer = new ReputationService(insertReport, selectReport, updateProfile, selectProfile, logger);
            ReputationManager repMan = new ReputationManager(repSer, logger);

            // Act
            var getReputation = repMan.ViewCurrentReputationAsync(1001);
            Console.WriteLine(getReputation.Result.Data);

            // Assert
            Assert.IsTrue(getReputation.Result.IsSuccessful);

        }
    }
}