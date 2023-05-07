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
        private readonly ILogger _logger;
        private readonly Response _result = new Response();

        public ReputationManager(ReputationService reputationService, ILogger logger)
        {
            _reputationService = reputationService;
            _logger = logger;
        }

        // TODO: Change ViewCurrentReputationAsync to return DataResponse with the proper datatype for the response
        public async Task<DataResponse<double>> ViewCurrentReputationAsync(string userHash, int user)
        {
            Log getReputationLog;
            DataResponse<double> result = new DataResponse<double>();

            var getReputation = await _reputationService.GetCurrentReputationAsync(user).ConfigureAwait(false);

            if (!getReputation.IsSuccessful)
            {
                result.ErrorMessage = getReputation.ErrorMessage;

                getReputationLog = new Log(1, "Error", userHash, "ReputationService.GetCurrentReputationAsync()", "Data", "Failed to retrieve users reputation from the data store");
                await _logger.Logs(getReputationLog).ConfigureAwait(false);

                return result;
            }
                result.Data = getReputation.Data.Reputation;
                getReputationLog = new Log(1, "Info", userHash, "ReputationService.GetCurrentReputationAsync()", "Data", "Successfully retrieved the users reputation from the data store.");    


            await _logger.Logs(getReputationLog).ConfigureAwait(false);

            return result;
        }

        public async Task<DataResponse<List<Reports>>> ViewUserReportsAsync(string userHash, int user, string btnCommand)
        {
            Log getReportsLog;
            DataResponse<List<Reports>> result = new DataResponse<List<Reports>>();

            var getReports = await _reputationService.GetUserReportsAsync(user, btnCommand).ConfigureAwait(false);

            if (!getReports.IsSuccessful)
            {
                getReportsLog = new Log(1, "Error", userHash, "ReputationService.GetUserReports()", "Data", "Failed to retrieve reports from the data store");

                await _logger.Logs(getReportsLog).ConfigureAwait(false);

                result.Data = getReports.Data;
                return result;

            }

            getReportsLog = new Log(1, "Info", userHash, "ReputationService.GetUserReports()", "Data", "Successfully retrieved reports from the data store");
            await _logger.Logs(getReportsLog).ConfigureAwait(false);

            result.IsSuccessful = true;
            result.Data = getReports.Data;

            return result;
        }
        
        public async Task<Response> IncreaseReputationByPointOneAsync(string userHash, int user)
        {
            Log getReputationLog;
            Log updateReputationLog;

            var getReputation = await _reputationService.GetCurrentReputationAsync(user).ConfigureAwait(false);

            if (!getReputation.IsSuccessful)
            {
                getReputationLog = new Log(1, "Error", userHash, "ReputationServices.GetCurrentReputation()",
                                           "Data", "Failed to retrieve current user reputation");

                await _logger.Logs(getReputationLog).ConfigureAwait(false);

                _result.ErrorMessage = getReputation.ErrorMessage;

                return _result;
            }

            double newReputation = (double)getReputation.Data.Reputation + 0.1;
            getReputationLog = new Log(1, "Info", userHash, "ReputationServices.GetCurrentReputation()", "Data", "Successfully retrieved current user reputation");


            var updateReputation = await _reputationService.UpdateReputationAsync(user, newReputation).ConfigureAwait(false);

            if (!updateReputation.IsSuccessful)
            {
                updateReputationLog = new Log(1, "Error", userHash, "ReputationServices.UpdateReputation()",
                                                "Data Store", "Failed to increase reputation by 0.1");

                await _logger.Logs(updateReputationLog).ConfigureAwait(false);

                return updateReputation;
            }

            updateReputationLog = new Log(1, "Info", userHash, "ReputationServices.UpdateReputation()", "Data Store", "Successfully increased reputation by 0.1");

            await _logger.Logs(updateReputationLog).ConfigureAwait(false);
            await _logger.Logs(getReputationLog).ConfigureAwait(false);

            return updateReputation;
        }

        public async Task<Response> RecordNewUserReportAsync(string userHash, Report report, double minimumRating)
        {
            Log getReputationLog;
            Log updateReputationLog;
            Log storeReportLog;
            Log updateRoleLog;
            string role = "Regular User";
            bool changeRole = false;
            var newReputation = await _reputationService.CalculateNewUserReputationAsync(report).ConfigureAwait(false);

            if(!newReputation.IsSuccessful)
            {
                Console.WriteLine("Failed to calculate reputation");
                getReputationLog = new Log(1, "Error", userHash, "ReputationServices.CalculateNewUserReputation()",
                 "Data", "Failed to retrieve user's new calculated reputation");

                await _logger.Logs(getReputationLog).ConfigureAwait(false);

                _result.ErrorMessage = newReputation.ErrorMessage;

                return _result;
            }
            getReputationLog = new Log(1, "Info", userHash, "ReputationServices.CalculateNewUserReputation()",
                                            "Data", "Successfully retrieved users new calculated reputation");

            var updateReputation = await _reputationService.UpdateReputationAsync(report.ReportedUser, newReputation.Data.Reputation).ConfigureAwait(false);
            if (!updateReputation.IsSuccessful)
            {
                Console.WriteLine("Failed to update reputation");
                _result.IsSuccessful = false;
                _result.ErrorMessage = updateReputation.ErrorMessage;
                updateReputationLog = new Log(1, "Error", userHash, "ReputationServices.UpdateReputation()", "Data Store", "Failed to update user's reputation");

                await _logger.Logs(updateReputationLog).ConfigureAwait (false);

                return _result;
            }


            if (newReputation.Data.Reputation >= minimumRating && newReputation.Data.Identity.AuthenticationType == "Regular User")
            {
                role = "Reputable User";
                changeRole = true;
            }
            else if (newReputation.Data.Reputation < minimumRating && newReputation.Data.Identity.AuthenticationType == "Reputable User")
            {
                role = "Regular User";
                changeRole = true;
            }


            if (changeRole)
            {
                var updateRole = await _reputationService.UpdateRoleAsync(newReputation.Data, role).ConfigureAwait(false);

                if (!updateRole.IsSuccessful)
                {
                    Console.WriteLine("Failed to update role");
                    updateRoleLog = new Log(1, "Error", userHash, "ReputationServices.UpdateRoleAsync()", "Data Store", $"Failed to update users role to {role}");
                    await _logger.Logs(updateRoleLog).ConfigureAwait(false);

                    return _result;
                }
                updateRoleLog = new Log(1, "Info", userHash, "ReputationServices.UpdateRoleAsync()", "Data Store", $"Successfully updated users role to {role}");
                await _logger.Logs(updateRoleLog) .ConfigureAwait (false);
            }

            updateReputationLog = new Log(1, "Info", userHash, "ReputationServices.UpdateReputation()", "Data Store", "Successfully updated users reputation");


            var storeReport = await _reputationService.StoreNewReportAsync(report).ConfigureAwait(false);
            if (!storeReport.IsSuccessful)
            {
                Console.WriteLine("Failed to Store Report");
                storeReportLog = new Log(1, "Error", userHash, "ReputationServices.StoreNewReport()", "Data Store", "Failed to store new report of the reported user");
                await _logger.Logs(storeReportLog).ConfigureAwait(false);

                return _result;
            }

            _result.IsSuccessful = true;
            storeReportLog = new Log(1,"Info",userHash,"ReputationServices.StoreNewReport()","Data Store","Successfully stored new report of the reported user");
            
            await _logger.Logs(storeReportLog).ConfigureAwait(false);          
            await _logger.Logs(updateReputationLog).ConfigureAwait(false);            
            await _logger.Logs(getReputationLog).ConfigureAwait(false);           

            return _result;
        }
    }
}