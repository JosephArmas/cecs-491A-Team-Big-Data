using TeamBigData.Utification.Services;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.Logging;
using ILogger = TeamBigData.Utification.Logging.Abstraction.ILogger;
using TeamBigData.Utification.Models;
using System.Security.Principal;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;

namespace TeamBigData.Utification.Manager
{
    public class ReputationManager
    {
        private readonly ReputationService _reputationService;
        private readonly Response _result = new Response();
        private readonly ILogger _logger;
        private UserAccount _userAccount = new UserAccount();
        private UserProfile _userProfile = new UserProfile();

        public ReputationManager(ReputationService reputationService, ILogger logger)
        {
            _reputationService = reputationService;
            _logger = logger;
        }

        public async Task<Response> ViewCurrentReputationAsync(UserProfile userProfile)
        {
            _userProfile = userProfile;
            Log getReputationLog;

            var getReputation = await _reputationService.GetCurrentReputationAsync(_userProfile).ConfigureAwait(false);

            if(getReputation.isSuccessful)
            {
                _result.data = getReputation.data;
                _result.isSuccessful = true;
                getReputationLog = new Log(1, "Info", _userAccount._userHash, "ReputationService.GetCurrentReputationAsync()", "Data", "Successfully retrieved the users reputation from the data store.");    
            }
            else
            {
                getReputationLog = new Log(1, "Error", _userAccount._userHash, "ReputationService.GetCurrentReputationAsync()", "Data", "Failed to retrieve users reputation from the data store");
            }

            await _logger.Logs(getReputationLog).ConfigureAwait(false);

            return _result;
        }

        public async Task<Response> ViewUserReportsAsync()
        {
            Log getReportsLog;

            var getReports = await _reputationService.GetUserReportsAsync(0).ConfigureAwait(false);

            if(getReports.isSuccessful) 
            {
                _result.data = getReports.data;
                getReportsLog = new Log(1, "Info", _userAccount._userHash, "ReputationService.GetUserReports()", "Data", "Successfully retrieved reports from the data store");
            }
            else
            {
                getReportsLog = new Log(1, "Error", _userAccount._userHash, "ReputationService.GetUserReports()", "Data", "Failed to retrieve reports from the data store");
            }

            await _logger.Logs(getReportsLog).ConfigureAwait(false);

            return _result;
        }

        public async Task<Response> IncreaseReputationByPointOneAsync(UserProfile userProfile)
        {
            _userProfile = userProfile;
            Log getReputationLog;
            Log updateReputationLog;

            var getReputation = await _reputationService.GetCurrentReputationAsync(_userProfile).ConfigureAwait(false);

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

                await _logger.Logs(updateReputationLog).ConfigureAwait(false);
            }
            else
            {
                getReputationLog = new Log(1, "Error", _userAccount._userHash, "ReputationServices.GetCurrentReputation()",
                                            "Data", "Failed to retrieve current user reputation");
            }

            await _logger.Logs(getReputationLog).ConfigureAwait(false);

            return _result;
        }

        public async Task<Response> RecordNewUserReportAsync(Report report, double minimumRating)
        {         
            Log feedbackValidationLog;
            Log getReputationLog;
            Log updateReputationLog;
            Log storeReportLog;
            Log updateRoleLog;

            Regex feedbackValidation = new Regex(@"^[a-zA-Z0-9\s.@áéíóúüñ¿¡ÁÉÍÓÚÜÑ-]*$");

            if (!(feedbackValidation.IsMatch(report._feedback) && report._feedback.Length > 7 && report._feedback.Length <= 150))
            {
                feedbackValidationLog = new Log(1, "Error", _userAccount._userHash, "Report Feedback Validation", "Business", "Users feedback violates validation check");
                _result.errorMessage = "Bad Request";
            }
            else
            {
                feedbackValidationLog = new Log(1, "Info", _userAccount._userHash, "Report Feedback Validation", "Business", "Users feedback passes validation check");

                var newReputation = await _reputationService.CalculateNewUserReputationAsync(report).ConfigureAwait(false);

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

                        var storeReport = await _reputationService.StoreNewReportAsync(report).ConfigureAwait(false);

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

                        await _logger.Logs(storeReportLog).ConfigureAwait(false);

                    }
                    else
                    {
                        _result.errorMessage = updateReputation.errorMessage;
                        updateReputationLog = new Log(1, "Error", _userAccount._userHash, "ReputationServices.UpdateReputation()",
                                                        "Data Store", "Failed to update user's reputation");
                    }

                    await _logger.Logs(updateReputationLog).ConfigureAwait(false);

                }
                else
                {
                    getReputationLog = new Log(1, "Error", _userAccount._userHash, "ReputationServices.CalculateNewUserReputation()",
                                                "Data", "Failed to retrieve user's new calculated reputation");
                }

                await _logger.Logs(getReputationLog).ConfigureAwait(false);
            }
            await _logger.Logs(feedbackValidationLog).ConfigureAwait(false);

            return _result;
        }
    }
}