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
        private readonly string featureString = @"Server=.\;Database=TeamBigData.Utification.Features;User=AppUser;Password=t;TrustServerCertificate=True";
        private readonly string userString = @"Server=.\;Database=TeamBigData.Utification.Users;User=AppUser;Password=t;TrustServerCertificate=True";
        private readonly string logString = @"Server=.;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True";
        private readonly ReputationService repSer;
        private readonly ReputationManager repMan;

        public ReputationIntegrationTest()
        {
            var reportsDAO = new ReportsSqlDAO(featureString);
            var usersDAO = new UsersSqlDAO(userString);
            var logger = new Logger(new LogsSqlDAO(logString));
            repSer = new ReputationService(reportsDAO, usersDAO, logger);
            repMan = new ReputationManager(repSer, logger);
        }

        [TestMethod]
        public async Task SubmitReportAndAffectReputation()
        {
            //Arrange
            // The report's user IDs vary upon the device and the users that are in the DB
            Report report = new Report(0.5, 1010, 1011, "This user sucks.");

            // Act
            var act = await repMan.RecordNewUserReportAsync(report, 4.2);

            Console.WriteLine(act.ErrorMessage);
            // Assert
            Assert.IsTrue(act.IsSuccessful);
        }

        [TestMethod]
        public async Task GetReports()
        {
            //Arrange
            // Act
            var getReports = await repSer.GetUserReportsAsync(1001, "");
              

            // Assert
            Assert.IsTrue(getReports.IsSuccessful);
        }

        [TestMethod]
        public async Task GetReputation()
        {
            //Arrange
            // Act
            var getReputation = await repMan.ViewCurrentReputationAsync(1001);

            // Assert
            Assert.IsTrue(getReputation.IsSuccessful);

        }
    }
}