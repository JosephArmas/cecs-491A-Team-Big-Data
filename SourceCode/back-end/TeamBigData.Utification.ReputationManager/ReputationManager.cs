﻿using TeamBigData.Utification.ReputationServices;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Models;
using System.Security.Principal;
using System.Linq.Expressions;

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

        public async Task<Response> ViewUserReports()
        {
            Log getReportsLog;

            var getReports = await _reputationService.GetUserReports(0).ConfigureAwait(false);

            if(getReports.isSuccessful) 
            {

            }
            return _result;
        }
        public async Task<Response> IncreaseReputationByPointOne()
        {
            Log getReputationLog;
            Log updateReputationLog;
            var getReputation = await _reputationService.GetCurrentReputation().ConfigureAwait(false);

            if (getReputation.isSuccessful)
            {
                double newReputation = (double)getReputation.data + 0.1;

                var updateReputation = await _reputationService.UpdateReputation(newReputation).ConfigureAwait(false);
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

        public async Task<Response> RecordNewUserReport(double minimumRating)
        {
            Log getReputationLog;
            Log updateReputationLog;
            Log storeReportLog;
            Log updateRoleLog;

            var newReputation = await _reputationService.CalculateNewUserReputation().ConfigureAwait(false);

            if (newReputation.isSuccessful)
            {

                getReputationLog = new Log(1, "Info", _userAccount._userHash, "ReputationServices.CalculateNewUserReputation()", 
                                            "Data", "Successfully retrieved users new calculated reputation");

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

                    updateReputationLog = new Log(1, "Info", _userAccount._userHash, "ReputationServices.UpdateReputation()", 
                                                    "Data Store", "Successfully updated users reputation");

                    var storeReport = await _reputationService.StoreNewReport().ConfigureAwait(false);

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
                getReputationLog= new Log(1, "Error", _userAccount._userHash, "ReputationServices.CalculateNewUserReputation()", 
                                            "Data", "Failed to retrieve user's new calculated reputation");
            }

            await _logger.Log(getReputationLog).ConfigureAwait(false);

            return _result;
        }
    }
}