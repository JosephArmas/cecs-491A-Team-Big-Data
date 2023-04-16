using TeamBigData.Utification.ReputationServices;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Models;
using System.Security.Principal;

namespace TeamBigData.Utification.Manager
{
    public class ReputationManager
    {
        private readonly ReputationService _reputationService;
        private readonly Response _result;
        private readonly Report _report;
        private readonly ILogger _logger;
        private UserAccount _userAccount;
        private UserProfile _userProfile;

        public ReputationManager(ReputationService reputationService, Response result, Report report, Logger logger, UserAccount userAccount, UserProfile userProfile)
        {
            _reputationService = reputationService;
            _result = result;
            _report = report;
            _logger = logger;
            _userAccount = userAccount;
            _userProfile = userProfile;
        }

        public async Task<Response> RecordNewUserReport(double minimumRating)
        {
            Log getReputationLog;
            Log updateReputationLog;
            Log storeReportLog;
            Log updateRoleLog;

            var newReputation = await _reputationService.GetUserReputationInfo().ConfigureAwait(false);

            if (newReputation.isSuccessful)
            {

                getReputationLog = new Log(1, "Info", _userAccount._userHash, "GetUpdatedTotalReputation()", "Data", "Successfully retrieved users new calculated reputation");
                _userProfile = newReputation.data as UserProfile;
                var updateReputation = await _reputationService.UpdateReputation(_userProfile._reputation).ConfigureAwait(false);

                if (updateReputation.isSuccessful)
                {

                    if (_userProfile._reputation >= minimumRating && _userProfile.Identity.AuthenticationType == "Regular User")
                    {
                        await _reputationService.UpdateRole(_userProfile, "Reputable User").ConfigureAwait(false);
                    }
                    else if (_userProfile._reputation < minimumRating && _userProfile.Identity.AuthenticationType == "Reputable User")
                    {
                        await _reputationService.UpdateRole(_userProfile, "Regular User").ConfigureAwait(false);
                    }

                    updateReputationLog = new Log(1, "Info", _userAccount._userHash, "UpdateReputation()", "Data Store", "Successfully updated users reputation");
                    var storeReport = await _reputationService.StoreNewReport().ConfigureAwait(false);

                    if (storeReport.isSuccessful)
                    {
                        _result.isSuccessful = storeReport.isSuccessful;
                        storeReportLog = new Log(1, "Info", _userAccount._userHash, "StoreNewReport()", "Data Store", "Successfully stored new report of the reported user");
                    }
                    else
                    {
                        storeReportLog = new Log(1, "Error", _userAccount._userHash, "StoreNewReport()", "Data Store", "Failed to store new report of the reported user");
                    }
                    await _logger.Log(storeReportLog).ConfigureAwait(false);
                }
                else
                {
                    updateReputationLog = new Log(1, "Error", _userAccount._userHash, "UpdateReputation()", "Data Store", "Failed to update user's reputation");
                }
                var updateLogResult = await _logger.Log(updateReputationLog).ConfigureAwait(false);
                Console.WriteLine($"updateReputation log = {updateLogResult.isSuccessful}, {updateLogResult.ToString()}");
            }
            else
            {
                getReputationLog= new Log(1, "Error", _userAccount._userHash, "GetUpdatedTotalReputation()", "Data", "Failed to retrieve user's new calculated reputation");
            }
            var reputationLogResult = await _logger.Log(getReputationLog).ConfigureAwait(false);
            Console.WriteLine(reputationLogResult.isSuccessful + reputationLogResult.ToString());
            return _result;
        }
    }
}