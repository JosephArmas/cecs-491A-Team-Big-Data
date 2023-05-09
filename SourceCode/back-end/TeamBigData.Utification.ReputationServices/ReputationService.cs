using System.Data;
using System.Security.Principal;
using Microsoft.Extensions.Configuration;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.Models.ControllerModels;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Reports;
using TeamBigData.Utification.SQLDataAccess.UsersDB;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;

namespace TeamBigData.Utification.ReputationServices
{
    public class ReputationService
    {
        private readonly IReportsDBInserter _insertReport;
        private readonly IReportsDBSelecter _selectReports;
        private readonly IUsersDBUpdater _updateUserProfile;
        private readonly IUsersDBSelecter _selectUserProfile;
        private readonly IConfiguration _configuration;
        public ReputationService(ReportsSqlDAO reportsSqlDAO, UsersSqlDAO userSqlDAO, IConfiguration configuration)
        {
            _insertReport = reportsSqlDAO;
            _selectReports = reportsSqlDAO;
            _updateUserProfile = userSqlDAO;
            _selectUserProfile = userSqlDAO;
            _configuration = configuration;
        }

        public async Task<DataResponse<List<Reports>>> GetUserReportsAsync(int user, string buttonCommand, int amount)
        {
            DataResponse<List<Reports>> result = new DataResponse<List<Reports>>();
            List<Reports> dataReports = new List<Reports>();
            var getReports = await _selectReports.SelectUserReportsAsync(user).ConfigureAwait(false);

            if (!getReports.IsSuccessful)
            {
                result.ErrorMessage = "Failed to retrieve user's reports";
                return result;
            }
            
            
            if(buttonCommand == "Next" && amount <= getReports.Data.Tables[0].Rows.Count)
            {
                amount += 5;
            }
            else if (buttonCommand == "Previous" && amount >= 5)
            {
                amount -= 5;
            }

            IEnumerable<DataRow> data = getReports.Data.Tables[0].AsEnumerable();
            int start = 0 + amount;
            int max = 5 + amount;
            Range range = new Range(start, max);
            foreach (DataRow report in data.Take(range))
            {
                Reports reports = new();
                reports.Rating = Decimal.ToDouble((decimal)report["rating"]);
                reports.Feedback = (string)report["feedback"];
                reports.CreateDate = ((DateTime)report["createDate"]).ToUniversalTime().ToString();
                reports.ReportingUserID = (int)report["reportingUserID"];
                dataReports.Add(reports);
            }

            result.IsSuccessful = true;
            result.Data = dataReports;


            return result;
        }

        // TODO: Change GetCurrentReputationAsync to return DataResponse with the proper datatype for the response
        public async Task<DataResponse<UserProfile>> GetCurrentReputationAsync(int user)
        {
            var getReputation = await _selectUserProfile.SelectUserProfile(user).ConfigureAwait(false);

            if(!getReputation.IsSuccessful)
            {
                getReputation.ErrorMessage += "Failed to retrieve user's current reputation";
            }
                        
            return getReputation;
        }

        public async Task<Response> UpdateRoleAsync(UserProfile userProfile, string role)
        {
            userProfile = new UserProfile(userProfile.UserID, role);

            var updateResult = await _updateUserProfile.UpdateUserRoleAsync(userProfile).ConfigureAwait(true);

            if (!updateResult.IsSuccessful)
            {
                updateResult.ErrorMessage = "Failed to update user role";
            }

            return updateResult;
        }

        public async Task<Response> UpdateReputationAsync(int user, double reputation)
        {
            var updateResult = await _updateUserProfile.UpdateUserReputationAsync(user, reputation).ConfigureAwait(false);

            if (!updateResult.IsSuccessful)
            {
                updateResult.ErrorMessage += "Failed to update user's reputation";
                return updateResult;
            }

            return updateResult;
        }

        public async Task<Response> CheckCompletionThresholdAsync(UserProfile userProfile)
        {

            Response result = new Response();

            if (userProfile.PinsCompleted == Convert.ToInt32(_configuration["Reputation:PinCompletionRewardCap"]))
            {
                result.ErrorMessage = "You have reached the maximum threshold to earn reputation";

                return result;
            }

            if (userProfile.Reputation == 5.0)
            {
                result.ErrorMessage = "You have the highest possible reputation.";

                return result;
            }
            Console.WriteLine("UserID to mark the tally for: " + userProfile.UserID);
            var updateResult = await _updateUserProfile.UpdatePinCompletionTallyAsync(userProfile.UserID, userProfile.PinsCompleted).ConfigureAwait(false);

            if (!updateResult.IsSuccessful)
            {
                result.ErrorMessage = "Failed to increase reputation by 0.1";
                return result;
            }

            updateResult.ErrorMessage = "Successfully increased reputation by 0.1";

            return updateResult;
        }
        /// <summary>
        /// Calculates a user's new reputation when a new report is submitted
        /// </summary>
        /// <returns><see cref="UserProfile"/></returns>

        // TODO: Change GetCurrentReputationAsync to return DataResponse with the proper datatype for the response
        public async Task<DataResponse<UserProfile>> CalculateNewUserReputationAsync(Report report)
        {            
            DataResponse<UserProfile> result = new DataResponse<UserProfile>();

            if (report.Rating > 5.0 || report.Rating < 0)
            {
                result.ErrorMessage = "Please enter a rating between 0.0 and 5.0";
                return result;
            }

            var getNewReputation = await _selectReports.SelectNewReputationAsync(report).ConfigureAwait(false);            

            if (!getNewReputation.IsSuccessful)
            {
               getNewReputation.ErrorMessage += "Failed to retrieve user's ratings";
               result.ErrorMessage = getNewReputation.ErrorMessage;

               return result;
            }

            var getOldReputation = await _selectUserProfile.SelectUserProfile(report.ReportedUser);

            if (!getOldReputation.IsSuccessful)
            {
                getOldReputation.ErrorMessage += "Failed to retrieve user's current reputation";
                result.ErrorMessage = getOldReputation.ErrorMessage;

                return result;
            }

            result.IsSuccessful = getOldReputation.IsSuccessful;

            double currentReputation = getOldReputation.Data.Reputation;
            double cumulativeRatings = (double)getNewReputation.Data.Item1;
            double numberOfReports = (double)getNewReputation.Data.Item2;

            double newReputation = (double)((currentReputation + cumulativeRatings) / (numberOfReports + 1));
            result.Data = new UserProfile(report.ReportedUser, newReputation, getOldReputation.Data.Identity.AuthenticationType);

            return result;
        }

        public async Task<Response> StoreNewReportAsync(Report report)
        {

            var insertReport = await _insertReport.InsertUserReportAsync(report).ConfigureAwait(false);

            if (!insertReport.IsSuccessful)
            {
                insertReport.ErrorMessage = "Failed to store report on user";
                return insertReport;
            }

            return insertReport;
        }

        public async Task<Response> ResetUserReputation(UserProfile userProfile, double defaultReputation)
        {
            if (!((IPrincipal)userProfile).IsInRole("Admin User"))
            {
                Response error = new Response();
                error.ErrorMessage = "Unauthorized Request";
                return error;
            }

            var updateResult = await _updateUserProfile.UpdateUserReputationAsync(userProfile.UserID, defaultReputation).ConfigureAwait(false);

            return updateResult;
        }
    }
}