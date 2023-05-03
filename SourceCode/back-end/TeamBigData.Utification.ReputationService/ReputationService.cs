using TeamBigData.Utification.Logging.Abstraction;
using System.Security.Principal;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.Models.ControllerModels;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Reports;
using ILogger = TeamBigData.Utification.Logging.Abstraction.ILogger;
using System.Data;
using TeamBigData.Utification.Logging;
using System.Globalization;

namespace TeamBigData.Utification.Services
{
    public class ReputationService
    {
        private readonly Response _result = new Response();
        private readonly IReportsDBInserter _insertReport;
        private readonly IReportsDBSelecter _selectReports;
        private readonly IUsersDBUpdater _updateUserProfile;
        private readonly IUsersDBSelecter _selectUserProfile;
        private readonly Report _report;
        private readonly ILogger _logger;
        private UserAccount _userAccount = new UserAccount();
        private UserProfile _userProfile = new UserProfile();
        public ReputationService(IReportsDBInserter insertReport, IReportsDBSelecter selectReports, IUsersDBUpdater updateUserProfile,
                    IUsersDBSelecter selectUserProfile, ILogger logger)
        {
            _insertReport = insertReport;
            _selectReports = selectReports;            
            _updateUserProfile = updateUserProfile;
            _selectUserProfile = selectUserProfile;
            _logger = logger;
        }

        public async Task<DataResponse<List<Reports>>> GetUserReportsAsync(int user, int amount)
        {
            DataResponse<List<Reports>> result = new DataResponse<List<Reports>>();
            List<Reports> dataReports = new List<Reports>();            
            var getReports = await _selectReports.SelectUserReportsAsync(user).ConfigureAwait(false);

            Console.WriteLine("If the row exists -> " + getReports.data.Tables[0].Rows.Count);

            if(!getReports.isSuccessful)
            {
                result.errorMessage = "Failed to retrieve user's reports";
                return result;
            }

            IEnumerable<DataRow> data = getReports.data.Tables[0].AsEnumerable();
            int start = 0 + amount;
            int max = 5 + amount;
            Range range = new Range(start, max);            

            foreach (DataRow report in data.Take(range))
            {
                Reports reports = new();
                reports.Rating = Decimal.ToDouble((decimal)report["rating"]);
                reports.Feedback = (string)report["feedback"];
                reports.CreateDate = ((DateTime)report["createDate"]).Date.ToString();
                reports.ReportingUserID = (int)report["reportingUserID"];
                dataReports.Add(reports);                    
            }

            for (int i = 0; i < dataReports.Count; i++)
            {
                Console.WriteLine(dataReports[i].Rating.ToString() + " " + dataReports[i].Feedback + " " + dataReports[i].CreateDate + " " + dataReports[i].ReportingUserID);
            }

            result.isSuccessful = true;
            result.data = dataReports;

            
            return result;
        }
        public async Task<Response> GetCurrentReputationAsync(int user)
        {
            var getReputation = await _selectUserProfile.SelectUserProfile(user).ConfigureAwait(false);
            
            _result.IsSuccessful = getReputation.isSuccessful;
            _result.Data = getReputation.data._reputation;

            return _result;
        }

        public async Task<Response> UpdateRoleAsync(UserProfile userProfile, string role)
        {
            Log log;
            _userProfile = new UserProfile(userProfile._userID, role);

            var update = await _updateUserProfile.UpdateUserRoleAsync(_userProfile).ConfigureAwait(true);

            if(!update.IsSuccessful)
            {
                log = new Log(1, "Error", _userAccount._userHash, "UpdateUserRole()", "Data Store", $"Failed to update users role to {role}");
            }

            log = new Log(1, "Info", _userAccount._userHash, "UpdateUserRole()", "Data Store", $"Successfully updated users role to {role}");            
            await _logger.Logs(log).ConfigureAwait(false);
            
            _result.IsSuccessful = true;

            return _result;
        }

        public async Task<Response> UpdateReputationAsync(int user, double reputation)
        {
            var update = await _updateUserProfile.UpdateUserReputationAsync(user, reputation).ConfigureAwait(false);

            if (!update.IsSuccessful)
            {
                _result.IsSuccessful = false;
                _result.ErrorMessage = "Failed to update user's reputation";
                return _result;
            }

            _result.IsSuccessful = true;

            return _result;
        }

        /// <summary>
        /// Calculates a user's new reputation when a new report is submitted
        /// </summary>
        /// <returns><see cref="UserProfile"/></returns>
        public async Task<Response> CalculateNewUserReputationAsync(Report report)
        {
            
            var getNewReputation = await _selectReports.SelectNewReputationAsync(report).ConfigureAwait(false);
            
            var getOldReputation = await _selectUserProfile.SelectUserProfile(report._reportedUser);                   
                        
            if (!getNewReputation.IsSuccessful)
            {
                _result.ErrorMessage = "Failed to retrieve user's ratings";
                return _result;
            }

            if (!getOldReputation.isSuccessful)
            {
                _result.ErrorMessage = "Failed to retrieve user's current reputation";
                return _result;
            }

            _result.IsSuccessful = getOldReputation.isSuccessful;

            var newRatings = getNewReputation.Data as Tuple<double, int>;

            double currentReputation = getOldReputation.data._reputation;
            double cumulativeRatings = (double)newRatings.Item1;
            double numberOfReports = (double)newRatings.Item2;

            double newReputation = (double)((currentReputation + cumulativeRatings) / (numberOfReports + 1));
            _result.Data = new UserProfile(report._reportedUser, newReputation, getOldReputation.data.Identity.AuthenticationType);

            return _result;
        }
                
        public async Task<Response> StoreNewReportAsync(Report report)
        {

            var insertReport = await _insertReport.InsertUserReportAsync(report).ConfigureAwait(false);

            if(!insertReport.IsSuccessful) 
            {
                _result.ErrorMessage = "Failed to store report on user";
                return _result;
            }

            _result.IsSuccessful = true;

            return _result;
        }

        public async Task<Response> ResetUserReputation()
        {
            if(!((IPrincipal)_userProfile).IsInRole("Admin User"))
            {
                _result.ErrorMessage = "Unauthorized Request";
                return _result;
            }

            await _updateUserProfile.UpdateUserReputationAsync(_userProfile._userID, 2.0).ConfigureAwait(false);
            
            return _result;
        }
    }
}