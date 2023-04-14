using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.Abstractions;

namespace TeamBigData.Utification.ReputationServices
{
    public class ReputationService
    {
        private readonly Response _result;
        private readonly IDBSelecter _selectReport;
        private readonly IDBInserter _insertReport;
        private readonly IDBUpdater _updateUserProfile;
        private readonly IDBSelecter _selectUserProfile;
        private readonly Report _report;
        private UserAccount _userAccount;
        private UserProfile _userProfile;
        public ReputationService(Response result, IDBSelecter selectReport, IDBInserter insertReport, IDBUpdater updateUserProfile, IDBSelecter selectUserProfile, Report report, UserAccount userAccount, UserProfile userProfile)
        {
            _result = result;
            _selectReport = selectReport;
            _insertReport = insertReport;
            _updateUserProfile = updateUserProfile;
            _selectUserProfile = selectUserProfile;
            _report = report; 
            _userAccount = userAccount; 
            _userProfile = userProfile;
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

        public async Task<Response> GetUpdatedTotalReputation()
        {
            var getNewReputation = await _selectReport.SelectNewReputation(_report).ConfigureAwait(false);
            
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

                    _result.data = (currentReputation + cumulativeRatings) / (numberOfReports + 1);
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