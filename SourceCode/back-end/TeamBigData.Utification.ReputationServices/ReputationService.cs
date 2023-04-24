using TeamBigData.Utification.Logging.Abstraction;
using System.Security.Principal;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using ILogger = TeamBigData.Utification.Logging.Abstraction.ILogger;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Reports;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using System.Data;

namespace TeamBigData.Utification.Services
{
    public class ReputationService
    {
        private readonly Response _result = new Response();
        private readonly IReportsDBInserter _insertReport;
        private readonly IReportsDBSelecter _selectReports;        
        private readonly IUsersDBUpdater _updateUserProfile;
        private readonly IUsersDBSelecter _selectUserProfile;
        private readonly ILogger _logger;
        private UserAccount _userAccount = new UserAccount();
        private UserProfile _userProfile = new UserProfile();
        public ReputationService(IReportsDBInserter insertReport, IReportsDBSelecter selectReports,  IUsersDBUpdater updateUserProfile,
                    IUsersDBSelecter selectUserProfile, ILogger logger)
        {
            _insertReport = insertReport;
            _selectReports = selectReports;            
            _updateUserProfile = updateUserProfile;
            _selectUserProfile = selectUserProfile;
            _logger = logger;
        }

        public async Task<Response> GetUserReportsAsync(int amount)
        {   
            List<Report> dataReports = new List<Report>();
            var getReports = await _selectReports.SelectUserReportsAsync(_userProfile).ConfigureAwait(false);

            if(getReports.data != null)
            {
               var reports = getReports.data as DataSet;

                IEnumerable<DataRow> data = reports.Tables[0].AsEnumerable();
                int start = 0 + amount;
                int max = 5 + amount;
                Range range = new Range(start, max);

                for(int i = 0; i < data.Count(); i++) 
                {
                    dataReports.Add(new Report(Decimal.ToDouble((decimal)reports.Tables[0].Rows[i][0]), (int)reports.Tables[0].Rows[i][1],
                        (int)reports.Tables[0].Rows[i][2], (string)reports.Tables[0].Rows[i][3], (DateTime)reports.Tables[0].Rows[i][4]));
                }

                _result.isSuccessful = true;
                _result.data = dataReports;
            }
            return _result;
        }
        public async Task<Response> GetCurrentReputationAsync(UserProfile userProfile)
        {
            _userProfile = userProfile;
            var getReputation = await _selectUserProfile.SelectUserProfile(_userProfile._userID).ConfigureAwait(false);

            if (getReputation.isSuccessful)
            {
                _result.isSuccessful = getReputation.isSuccessful;
                _result.data = _userProfile._reputation;
            }
            else 
            {
                _result.errorMessage = "Reputation is Unavailable Right Now. Please Try Again";
            }
            return _result;
        }

        public async Task<Response> UpdateRoleAsync(UserProfile userProfile, string role)
        {
            Log log;
            _userProfile = new UserProfile(userProfile._userID, role);

            var update = await _updateUserProfile.UpdateUserRoleAsync(_userProfile).ConfigureAwait(true);

            if(update.isSuccessful)
            {
                log = new Log(1, "Info", _userAccount._userHash, "UpdateUserRole()", "Data Store", $"Successfully updated users role to {role}");
                _result.isSuccessful = true;
            }
            else 
            {
                log = new Log(1, "Error", _userAccount._userHash, "UpdateUserRole()", "Data Store", $"Failed to update users role to {role}");
            }

            await _logger.Log(log).ConfigureAwait(false);

            return _result;
        }

        public async Task<Response> UpdateReputationAsync(double reputation)
        {
            var update = await _updateUserProfile.UpdateUserReputationAsync(_userProfile, reputation).ConfigureAwait(false);

            if (update.isSuccessful)
            {
                _result.isSuccessful = true;
            }
            else 
            {
                _result.errorMessage = "Report Failed to Submit. Please Try Again";
            }
            
            return _result;
        }

        /// <summary>
        /// Calculates a user's new reputation when a new report is submitted
        /// </summary>
        /// <returns><see cref="UserProfile"/></returns>
        public async Task<Response> CalculateNewUserReputationAsync(Report report)
        {
            
            var getNewReputation = await _selectReports.SelectNewReputationAsync(report).ConfigureAwait(false);

            var getOldReputation = await _selectUserProfile.SelectUserProfile(_userAccount._userID).ConfigureAwait(false);
                        
            if (getNewReputation.isSuccessful)
            {               
                if (getOldReputation.isSuccessful)
                {                    
                    _result.isSuccessful = getOldReputation.isSuccessful;

                    var newRatings = getNewReputation.data as Tuple<double, int>;

                    double currentReputation = _userProfile._reputation;
                    double cumulativeRatings = (double)newRatings.Item1;
                    double numberOfReports = (double)newRatings.Item2;

                    double newReputation = (double)((currentReputation + cumulativeRatings) / (numberOfReports + 1));
                    _result.data = new UserProfile(getOldReputation.data._userID, newReputation, getOldReputation.data.Identity.AuthenticationType);
                }               
            }

            return _result;
        }
                
        public async Task<Response> StoreNewReportAsync(Report report)
        {
            var insertReport = await _insertReport.InsertUserReportAsync(report).ConfigureAwait(false);

            if(insertReport.isSuccessful) 
            {
                _result.isSuccessful = true;
            }

            return _result;
        }
    }
}