﻿using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.Models.ControllerModels;
using ILogger = TeamBigData.Utification.Logging.Abstraction.ILogger;
using System.Security.Principal;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.ReputationServices;
using TeamBigData.Utification.Logging;

namespace TeamBigData.Utification.Manager
{
    public class ReputationManager
    {
        private readonly ReputationService _reputationService;
        private readonly Response _result = new Response();
        //private readonly Report _report;
        private readonly ILogger _logger;
        //private UserAccount _userAccount;
        //private UserProfile _userProfile;

        // try passing just the values needed of the user instead of creating the user to make checks on

        public ReputationManager(ReputationService reputationService, ILogger logger)
        {
            _reputationService = reputationService;
            _logger = logger;
        }

        // TODO: Change ViewCurrentReputationAsync to return DataResponse with the proper datatype for the response
        public async Task<Response> ViewCurrentReputationAsync(int user)
        {
            Log getReputationLog;

            var getReputation = await _reputationService.GetCurrentReputationAsync(user).ConfigureAwait(false);

            if (getReputation.IsSuccessful)
            {
                _result.IsSuccessful = true;
                // _result.Data = getReputation.Data;
                //getReputationLog = new Log(1, "Info", user, "ReputationService.GetCurrentReputationAsync()", "Data", "Successfully retrieved the users reputation from the data store.");    
            }
            else
            {
                //getReputationLog = new Log(1, "Error", _userAccount._userHash, "ReputationService.GetCurrentReputationAsync()", "Data", "Failed to retrieve users reputation from the data store");
            }

            //await _logger.Logs(getReputationLog).ConfigureAwait(false);

            return _result;
        }

        
        // functions inside are throwing errors
        public async Task<DataResponse<List<Reports>>> ViewUserReportsAsync(int user, string btnCommand)
        {
            /*
            Log getReportsLog;
            DataResponse<List<Reports>> result = new DataResponse<List<Reports>>();

            var getReports = await _reputationService.GetUserReportsAsync(user, btnCommand).ConfigureAwait(false);

            if (!getReports.isSuccessful)
            {
                getReportsLog = new Log(1, "Error", _userAccount.UserHash, "ReputationService.GetUserReports()", "Data", "Failed to retrieve reports from the data store");

                await _logger.Logs(getReportsLog).ConfigureAwait(false);

                result.Data = getReports.data;
                return result;

            }

            //getReportsLog = new Log(1, "Info", _userAccount._userHash, "ReputationService.GetUserReports()", "Data", "Successfully retrieved reports from the data store");
            //await _logger.Logs(getReportsLog).ConfigureAwait(false);

            result.IsSuccessful = true;
            result.Data = getReports.data;

            return result;*/

            throw new NotImplementedException();
        }
        // functions inside are throwing errors
        public async Task<Response> IncreaseReputationByPointOneAsync(int user)
        {
            /*
            Log getReputationLog;
            Log updateReputationLog;

            var getReputation = await _reputationService.GetCurrentReputationAsync(user).ConfigureAwait(false);

            if (!getReputation.IsSuccessful)
            {
                getReputationLog = new Log(1, "Error", _userAccount._userHash, "ReputationServices.GetCurrentReputation()",
                                           "Data", "Failed to retrieve current user reputation");

                await _logger.Logs(getReputationLog).ConfigureAwait(false);

                return _result;
            }

            double newReputation = (double)getReputation.Data + 0.1;
            getReputationLog = new Log(1, "Info", _userAccount._userHash, "ReputationServices.GetCurrentReputation()", "Data", "Successfully retrieved current user reputation");


            var updateReputation = await _reputationService.UpdateReputationAsync(user, newReputation).ConfigureAwait(false);

            if (!updateReputation.IsSuccessful)
            {
                updateReputationLog = new Log(1, "Error", _userAccount._userHash, "ReputationServices.UpdateReputation()",
                                                "Data Store", "Failed to increase reputation by 0.1");

                await _logger.Logs(updateReputationLog).ConfigureAwait(false);

                return _result;
            }

            _result.IsSuccessful = true;

            updateReputationLog = new Log(1, "Info", _userAccount._userHash, "ReputationServices.UpdateReputation()", "Data Store", "Successfully increased reputation by 0.1");

            await _logger.Logs(updateReputationLog).ConfigureAwait(false);
            await _logger.Logs(getReputationLog).ConfigureAwait(false);

            return _result;*/


            throw new NotImplementedException();
        }
        

        // functions inside are throwing errors
        public async Task<Response> RecordNewUserReportAsync(Report report, double minimumRating)
        {
            /*
            Log feedbackValidationLog;
            Log getReputationLog;
            Log updateReputationLog;
            Log storeReportLog;
            Log updateRoleLog;
            string role = "Regular User";
            bool changeRole = false;

            //Regex feedbackValidation = new Regex(@"^[a-zA-Z0-9]*$");            

            /*if (!(feedbackValidation.IsMatch(report._feedback) && report._feedback.Length > 7 && report._feedback.Length <= 150))
            {
                //feedbackValidationLog = new Log(1, "Error", _userAccount._userHash, "Report Feedback Validation", "Business", "Users feedback violates validation check");
                _result.ErrorMessage = "Bad Request";

                //await _logger.Logs(feedbackValidationLog).ConfigureAwait(false);

                return _result;
            }

            //feedbackValidationLog = new Log(1, "Info", _userAccount._userHash, "Report Feedback Validation", "Business", "Users feedback passes validation check");
            var newReputation = await _reputationService.CalculateNewUserReputationAsync(report).ConfigureAwait(false);

            if (!newReputation.IsSuccessful)
            {
                Console.WriteLine("Failed to calculate reputation");
                //getReputationLog = new Log(1, "Error", _userAccount._userHash, "ReputationServices.CalculateNewUserReputation()",
                // "Data", "Failed to retrieve user's new calculated reputation");

                //await _logger.Logs(getReputationLog).ConfigureAwait(false);

                return _result;
            }
            /*getReputationLog = new Log(1, "Info", _userAccount._userHash, "ReputationServices.CalculateNewUserReputation()",
                                            "Data", "Successfully retrieved users new calculated reputation");

            _userProfile = newReputation.Data as UserProfile;
            Console.WriteLine(_userProfile.ToString());

            var updateReputation = await _reputationService.UpdateReputationAsync(report.ReportedUser, _userProfile.Reputation).ConfigureAwait(false);
            if (!updateReputation.IsSuccessful)
            {
                Console.WriteLine("Failed to update reputation");
                _result.IsSuccessful = false;
                _result.ErrorMessage = updateReputation.ErrorMessage;
                //updateReputationLog = new Log(1, "Error", _userAccount._userHash, "ReputationServices.UpdateReputation()", "Data Store", "Failed to update user's reputation");

                //await _logger.Logs(updateReputationLog).ConfigureAwait (false);

                return _result;
            }


            if (_userProfile.Reputation >= minimumRating && _userProfile.Identity.AuthenticationType == "Regular User")
            {
                role = "Reputable User";
                changeRole = true;
            }
            else if (_userProfile.Reputation < minimumRating && _userProfile.Identity.AuthenticationType == "Reputable User")
            {
                role = "Regular User";
                changeRole = true;
            }


            if (changeRole)
            {
                var updateRole = await _reputationService.UpdateRoleAsync(_userProfile, role).ConfigureAwait(false);

                if (!updateRole.IsSuccessful)
                {
                    Console.WriteLine("Failed to update role");
                    updateRoleLog = new Log(1, "Error", _userAccount.UserHash, "ReputationServices.UpdateRoleAsync()", "Data Store", $"Failed to update users role to {role}");
                    await _logger.Logs(updateRoleLog).ConfigureAwait(false);

                    return _result;
                }
                //updateRoleLog = new Log(1, "Info", _userAccount._userHash, "ReputationServices.UpdateRoleAsync()", "Data Store", $"Successfully updated users role to {role}");
                //await _logger.Logs(updateRoleLog) .ConfigureAwait (false);
            }

            //updateReputationLog = new Log(1, "Info", _userAccount._userHash, "ReputationServices.UpdateReputation()", "Data Store", "Successfully updated users reputation");


            var storeReport = await _reputationService.StoreNewReportAsync(report).ConfigureAwait(false);
            if (!storeReport.IsSuccessful)
            {
                Console.WriteLine("Failed to Store Report");
                storeReportLog = new Log(1, "Error", _userAccount.UserHash, "ReputationServices.StoreNewReport()", "Data Store", "Failed to store new report of the reported user");
                await _logger.Logs(storeReportLog).ConfigureAwait(false);

                return _result;
            }

            _result.IsSuccessful = true;
            //storeReportLog = new Log(1,"Info",_userAccount._userHash,"ReputationServices.StoreNewReport()","Data Store","Successfully stored new report of the reported user");

            /*await _logger.Logs(storeReportLog).ConfigureAwait(false);          
            await _logger.Logs(updateReputationLog).ConfigureAwait(false);            
            await _logger.Logs(getReputationLog).ConfigureAwait(false);            
            await _logger.Logs(feedbackValidationLog).ConfigureAwait(false);

            return _result;
            */

            throw new NotImplementedException();
        }
    }
}