using TeamBigData.Utification.Logging.Abstraction;
using System.Security.Principal;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using System.Data;

namespace TeamBigData.Utification.ReputationServices
{
    public class ReputationService
    {
        private readonly Response _result;
        private readonly IDBSelecter _selectReports;
        private readonly IDBInserter _insertReport;
        private readonly IDBUpdater _updateUserProfile;
        private readonly IDBSelecter _selectUserProfile;
        private readonly Report _report;
        private readonly ILogger _logger;
        private UserAccount _userAccount;
        private UserProfile _userProfile;
        public ReputationService(Response result, IDBSelecter selectReports, IDBInserter insertReport, IDBUpdater updateUserProfile,
                    IDBSelecter selectUserProfile, Report report, UserAccount userAccount, UserProfile userProfile, ILogger logger)
        {
            _result = result;            
            _selectReports = selectReports;
            _insertReport = insertReport;
            _updateUserProfile = updateUserProfile;
            _selectUserProfile = selectUserProfile;
            _report = report;
            _logger = logger;
            _userAccount = userAccount; 
            _userProfile = userProfile;
        }

        public async Task<Response> GetUserReports(int amount)
        {
            List<DataRow> dataRows = new List<DataRow>();
            var getReports = await _selectReports.SelectUserReports(_userProfile).ConfigureAwait(false);

            if(getReports.data != null)
            {
               var reports = getReports.data as DataSet;

                IEnumerable<DataRow> data = reports.Tables[0].AsEnumerable();
                int start = 0 + amount;
                int max = 5 + amount;
                Range range = new Range(start, max);
                foreach (DataRow report in data.Take(range))
                {
                    Console.WriteLine($"Report: {report[0].ToString()}, {report[1].ToString()}");
                }
                _result.isSuccessful = true;

            }
            return _result;
        }
        public async Task<Response> GetCurrentReputation()
        {
            var getReputation = await _selectUserProfile.SelectUserProfile(ref _userProfile, _userAccount._userID).ConfigureAwait(false);
            
            _result.isSuccessful = getReputation.isSuccessful;
            _result.data = _userProfile._reputation;

            return _result;
        }

        public async Task<Response> UpdateRole(UserProfile userProfile, string role)
        {
            Log log;
            _userProfile = new UserProfile(userProfile._userID, role);

            var update = await _updateUserProfile.UpdateUserRole(_userProfile).ConfigureAwait(true);

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

        public async Task<Response> UpdateReputation(double reputation)
        {
            var update = await _updateUserProfile.UpdateUserReputation(_userProfile, reputation).ConfigureAwait(false);

            if (update.isSuccessful)
            {
                _result.isSuccessful = true;
            }                     
            
            return _result;
        }

        /// <summary>
        /// Calculates a user's new reputation when a new report is submitted
        /// </summary>
        /// <returns><see cref="UserProfile"/></returns>
        public async Task<Response> CalculateNewUserReputation()
        {
            
            var getNewReputation = await _selectReports.SelectNewReputation(_report).ConfigureAwait(false);
            
            var getOldReputation = _selectUserProfile.SelectUserProfile(ref _userProfile, _userAccount._userID);                   
                        
            if (getNewReputation.isSuccessful)
            {               
                if (getOldReputation.Result.isSuccessful)
                {                    
                    _result.isSuccessful = getOldReputation.Result.isSuccessful;

                    var newRatings = getNewReputation.data as Tuple<double, int>;

                    double currentReputation = _userProfile._reputation;
                    double cumulativeRatings = (double)newRatings.Item1;
                    double numberOfReports = (double)newRatings.Item2;

                    double newReputation = (double)((currentReputation + cumulativeRatings) / (numberOfReports + 1));
                    _result.data = new UserProfile(_userProfile._userID, newReputation, _userProfile.Identity.AuthenticationType);
                }               
            }

            return _result;
        }
                
        public async Task<Response> StoreNewReport()
        {
            var insertReport = await _insertReport.InsertUserReport(_report).ConfigureAwait(false);

            if(insertReport.isSuccessful) 
            {
                _result.isSuccessful = true;
            }

            return _result;
        }
    }
}