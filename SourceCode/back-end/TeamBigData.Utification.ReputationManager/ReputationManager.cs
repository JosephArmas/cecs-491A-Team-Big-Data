using TeamBigData.Utification.Services;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Models;
using System.Security.Principal;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

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

        public async Task<Response> ViewUserReportsAsync()
        {
            Log getReportsLog;

            var getReports = await _reputationService.GetUserReportsAsync(0).ConfigureAwait(false);

            if(getReports.isSuccessful) 
            {
                getReportsLog = new Log(1, "Info", _userAccount._userHash, "ReputationService.GetUserReports()", "Data", "Successfully retrieved reports from the data store");
            }
            else
            {
                getReportsLog = new Log(1, "Error", _userAccount._userHash, "ReputationService.GetUserReports()", "Data", "Failed to retrieve reports from the data store");
            }

            await _logger.Log(getReportsLog).ConfigureAwait(false);

            return _result;
        }
        public async Task<Response> IncreaseReputationByPointOneAsync()
        {
            Log getReputationLog;
            Log updateReputationLog;
            var getReputation = await _reputationService.GetCurrentReputationAsync().ConfigureAwait(false);

            if (getReputation.isSuccessful)
            {
                double newReputation = (double)getReputation.data + 0.1;

                var updateReputation = await _reputationService.UpdateReputationAsync(newReputation).ConfigureAwait(false);
                getReputationLog = new Log(1, "Info", _userAccount._userHash, "ReputationServices.GetCurrentReputation()", 
                                            "Data", "Successfully retrieved current user reputation");
                
                if(updateReputation.isSuccessful)
                {
                    _result.isSuccessful = true;
                    updateReputationLog = new Log(1, "Info", _userAccount._userHash, "ReputationServices.UpdateReputation()", 
                                                    "Data Store", "Successfully increased reputation by 0.1");
                }
                else 
                {
                    updateReputationLog = new Log(1, "Error", _userAccount._userHash, "ReputationServices.UpdateReputation()", 
                                                    "Data Store", "Failed to increase reputation by 0.1");
                }

                await _logger.Log(updateReputationLog).ConfigureAwait(false);
            }
            else
            {
                getReputationLog = new Log(1, "Error", _userAccount._userHash, "ReputationServices.GetCurrentReputation()",
                                            "Data", "Failed to retrieve current user reputation");
            }

            await _logger.Log(getReputationLog).ConfigureAwait(false);

            return _result;
        }

        public async Task<Response> RecordNewUserReportAsync(double minimumRating)
        {
            Log feedbackValidationLog;
            Log getReputationLog;
            Log updateReputationLog;
            Log storeReportLog;
            Log updateRoleLog;

            Regex feedbackValidation = new Regex(@"^[a-zA-Z0-9\s.@áéíóúüñ¿¡ÁÉÍÓÚÜÑ-]*$");

            if (!(feedbackValidation.IsMatch(_report._feedback) && _report._feedback.Length > 7 && _report._feedback.Length < 151))
            {
                feedbackValidationLog = new Log(1, "Error", _userAccount._userHash, "Report Feedback Validation", "Business", "Users feedback violates validation check");
                _result.errorMessage = feedbackValidation.Match(_report._feedback).ToString();
            }
            else
            {
                feedbackValidationLog = new Log(1, "Info", _userAccount._userHash, "Report Feedback Validation", "Business", "Users feedback passes validation check");

                var newReputation = await _reputationService.CalculateNewUserReputationAsync().ConfigureAwait(false);

                if (newReputation.isSuccessful)
                {

                    getReputationLog = new Log(1, "Info", _userAccount._userHash, "ReputationServices.CalculateNewUserReputation()",
                                                "Data", "Successfully retrieved users new calculated reputation");

                    _userProfile = newReputation.data as UserProfile;
                    var updateReputation = await _reputationService.UpdateReputationAsync(_userProfile._reputation).ConfigureAwait(false);

                    if (updateReputation.isSuccessful)
                    {

                        if (_userProfile._reputation >= minimumRating && _userProfile.Identity.AuthenticationType == "Regular User")
                        {
                            await _reputationService.UpdateRoleAsync(_userProfile, "Reputable User").ConfigureAwait(false);
                        }
                        else if (_userProfile._reputation < minimumRating && _userProfile.Identity.AuthenticationType == "Reputable User")
                        {
                            await _reputationService.UpdateRoleAsync(_userProfile, "Regular User").ConfigureAwait(false);
                        }

                        updateReputationLog = new Log(1, "Info", _userAccount._userHash, "ReputationServices.UpdateReputation()",
                                                        "Data Store", "Successfully updated users reputation");

                        var storeReport = await _reputationService.StoreNewReportAsync().ConfigureAwait(false);

                        if (storeReport.isSuccessful)
                        {
                            _result.isSuccessful = storeReport.isSuccessful;

                            storeReportLog = new Log(1, "Info", _userAccount._userHash, "ReputationServices.StoreNewReport()",
                                                        "Data Store", "Successfully stored new report of the reported user");
                        }
                        else
                        {
                            storeReportLog = new Log(1, "Error", _userAccount._userHash, "ReputationServices.StoreNewReport()",
                                                        "Data Store", "Failed to store new report of the reported user");
                        }

                        await _logger.Log(storeReportLog).ConfigureAwait(false);

                    }
                    else
                    {
                        updateReputationLog = new Log(1, "Error", _userAccount._userHash, "ReputationServices.UpdateReputation()",
                                                        "Data Store", "Failed to update user's reputation");
                    }

                    await _logger.Log(updateReputationLog).ConfigureAwait(false);

                }
                else
                {
                    getReputationLog = new Log(1, "Error", _userAccount._userHash, "ReputationServices.CalculateNewUserReputation()",
                                                "Data", "Failed to retrieve user's new calculated reputation");
                }

                await _logger.Log(getReputationLog).ConfigureAwait(false);
            }
            await _logger.Log(feedbackValidationLog).ConfigureAwait(false);

            return _result;
        }
    }
}