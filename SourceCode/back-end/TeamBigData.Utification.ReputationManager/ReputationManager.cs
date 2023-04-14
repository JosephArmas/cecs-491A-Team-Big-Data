using TeamBigData.Utification.ReputationServices;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.Manager
{
    public class ReputationManager
    {
        private readonly ReputationService _reputationService;
        private readonly Response _result;
        private readonly ILogger _logger;
        private UserAccount _userAccount;

        public ReputationManager(ReputationService reputationService, Response result, ILogger logger, UserAccount userAccount)
        {
            _reputationService = reputationService;
            _result = result;
            _logger = logger;
            _userAccount = userAccount;
        }

        public async Task<Response> RecordNewUserReport()
        {
            var newReputation = await _reputationService.GetUpdatedTotalReputation();

            if (newReputation.isSuccessful) 
            {
                await _logger.Log(new Log(1, "Info", _userAccount._userHash, "GetUpdatedTotalReputation()", "Data", "Successfully retrieved user's new calculated reputation"));

                var updateReputation = await _reputationService.UpdateReputation((double)newReputation.data);

                if(updateReputation.isSuccessful)
                {
                    await _logger.Log(new Log(1, "Info", _userAccount._userHash, "UpdateReputation()", "Data Store", "Successfully updated user's reputation"));

                    var storeReport = await _reputationService.StoreNewReport();

                    if(storeReport.isSuccessful)
                    {
                        await _logger.Log(new Log(1, "Info", _userAccount._userHash, "StoreNewReport()", "Data Store", "Successfully stored new report of the reported user"));

                        _result.isSuccessful = storeReport.isSuccessful;
                    }
                    else
                    {
                        await _logger.Log(new Log(1, "Error", _userAccount._userHash, "StoreNewReport()", "Data Store", "Failed to store new report of the reported user"));
                    }
                }
                else
                {
                    await _logger.Log(new Log(1, "Error", _userAccount._userHash, "UpdateReputation()", "Data Store", "Failed to update user's reputation"));
                }
            }
            else
            {
                await _logger.Log(new Log(1, "Error", _userAccount._userHash, "GetUpdatedTotalReputation()", "Data", "Failed to retrieve user's new calculated reputation"));
            }
                        
            return _result;
        }
    }
}