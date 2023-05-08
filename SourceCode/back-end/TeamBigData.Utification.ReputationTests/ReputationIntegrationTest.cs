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
            ReportsSqlDAO reportsDAO = new ReportsSqlDAO(@"Server=.\;Database=TeamBigData.Utification.Features;User=AppUser;Password=t;TrustServerCertificate=True");
            UsersSqlDAO usersDAO = new UsersSqlDAO(@"Server=.\;Database=TeamBigData.Utification.Users;User=AppUser;Password=t;TrustServerCertificate=True");
            ILogger logger = new Logger(new LogsSqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            ReputationService repSer = new ReputationService(reportsDAO, usersDAO);
            ReputationManager repMan = new ReputationManager(repSer, logger);
            // The report's user IDs vary upon the device and the users that are in the DB
            Report report = new Report(0.5, 1010, 1011, "This user sucks.");

            // Act
            var act = repMan.RecordNewUserReportAsync("A2-C6-C9-5C-77-40-3F-ED-C3-45-37-ED-80-BE-B8-A7-6D-26-62-E8-49-6F-50-70-25-79-56-B9-CD-70-54-21-EA-8E-24-D7-73-E5-B8-B2-63-F8-E6-4C-7A-1C-AC-90-CD-3D-EA-F5-0A-4A-85-CE-EA-D6-13-26-69-2B-80-48", report, 4.2);

            // Assert
            Assert.IsTrue(act.Result.IsSuccessful);
        }

        [TestMethod]
        public void GetReports()
        {
            // Arrange
            ReportsSqlDAO reportsDAO = new ReportsSqlDAO(@"Server=.\;Database=TeamBigData.Utification.Features;User=AppUser;Password=t;TrustServerCertificate=True");
            UsersSqlDAO usersDAO = new UsersSqlDAO(@"Server=.\;Database=TeamBigData.Utification.Users;User=AppUser;Password=t;TrustServerCertificate=True");
            ILogger logger = new Logger(new LogsSqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            ReputationService repSer = new ReputationService(reportsDAO, usersDAO);
            ReputationManager repMan = new ReputationManager(repSer, logger);
            
            // Act
            var getReports = repSer.GetUserReportsAsync(1001, "");
              

            // Assert
            Assert.IsTrue(getReports.Result.IsSuccessful);
        }

        [TestMethod]
        public void GetReputation()
        {
            // Arrange
            ReportsSqlDAO reportsDAO = new ReportsSqlDAO(@"Server=.\;Database=TeamBigData.Utification.Features;User=AppUser;Password=t;TrustServerCertificate=True");
            UsersSqlDAO usersDAO = new UsersSqlDAO(@"Server=.\;Database=TeamBigData.Utification.Users;User=AppUser;Password=t;TrustServerCertificate=True");
            ILogger logger = new Logger(new LogsSqlDAO(@"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            ReputationService repSer = new ReputationService(reportsDAO, usersDAO);
            ReputationManager repMan = new ReputationManager(repSer, logger);

            // Act
            var getReputation = repMan.ViewCurrentReputationAsync("A2-C6-C9-5C-77-40-3F-ED-C3-45-37-ED-80-BE-B8-A7-6D-26-62-E8-49-6F-50-70-25-79-56-B9-CD-70-54-21-EA-8E-24-D7-73-E5-B8-B2-63-F8-E6-4C-7A-1C-AC-90-CD-3D-EA-F5-0A-4A-85-CE-EA-D6-13-26-69-2B-80-48", 1001);

            // Assert
            Assert.IsTrue(getReputation.Result.IsSuccessful);

        }
    }
}