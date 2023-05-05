﻿using System.Data;
using System.Security.Principal;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Reports;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;

namespace TeamBigData.Utification.ReputationServices
{
    public class ReputationService
    {
        private readonly Response _result = new Response();
        private readonly IReportsDBInserter _insertReport;
        private readonly IReportsDBSelecter _selectReports;
        private readonly IUsersDBUpdater _updateUserProfile;
        private readonly IUsersDBSelecter _selectUserProfile;
        private readonly Report _report;
        private readonly ILogger _logger;
        private UserAccount _userAccount = new UserAccount();
        private UserProfile _userProfile = new UserProfile();
        public ReputationService(IReportsDBInserter insertReport, IReportsDBSelecter selectReports, IUsersDBUpdater updateUserProfile,
                    IUsersDBSelecter selectUserProfile, ILogger logger)
        {
            _insertReport = insertReport;
            _selectReports = selectReports;
            _updateUserProfile = updateUserProfile;
            _selectUserProfile = selectUserProfile;
            _logger = logger;
        }

        public async Task<DataResponse<List<DataRow>>> GetUserReportsAsync(int amount)
        {
            DataResponse<List<DataRow>> result = new DataResponse<List<DataRow>>();
            List<DataRow> dataReports = new List<DataRow>();
            var getReports = await _selectReports.SelectUserReportsAsync(_userProfile).ConfigureAwait(false);

            if (!getReports.IsSuccessful)
            {
                result.ErrorMessage = "Failed to retrieve user's reports";
                return result;
            }

            IEnumerable<DataRow> data = getReports.Data.Tables[0].AsEnumerable();
            int start = 0 + amount;
            int max = 5 + amount;
            Range range = new Range(start, max);

            foreach (DataRow report in data.Take(range))
            {
                Console.WriteLine(report.ToString());
                dataReports.Add(report);
            }
            result.IsSuccessful = true;
            result.Data = dataReports;

            for (int i = 0; i < dataReports.Count; i++)
            {
                Console.WriteLine("Yup, it's loopin");
                Console.WriteLine(dataReports[i].Table);
            }
            return result;
        }
        public async Task<DataResponse<double>> GetCurrentReputationAsync(int user)
        {
            var getReputation = await _selectUserProfile.SelectUserProfile(user).ConfigureAwait(false);

            DataResponse<double> data = new DataResponse<double>(getReputation.IsSuccessful, getReputation.ErrorMessage, getReputation.Data.Reputation);

            return data;
        }

        public async Task<Response> UpdateRoleAsync(UserProfile userProfile, string role)
        {
            Log log;
            _userProfile = new UserProfile(userProfile.UserID, role);

            var update = await _updateUserProfile.UpdateUserRoleAsync(_userProfile).ConfigureAwait(true);

            if (!update.IsSuccessful)
            {
                log = new Log(1, "Error", _userAccount.UserHash, "UpdateUserRole()", "Data Store", $"Failed to update users role to {role}");
            }

            log = new Log(1, "Info", _userAccount.UserHash, "UpdateUserRole()", "Data Store", $"Successfully updated users role to {role}");
            await _logger.Logs(log).ConfigureAwait(false);

            _result.IsSuccessful = true;

            return _result;
        }

        public async Task<Response> UpdateReputationAsync(double reputation)
        {
            var update = await _updateUserProfile.UpdateUserReputationAsync(_userProfile, reputation).ConfigureAwait(false);

            if (!update.IsSuccessful)
            {
                _result.ErrorMessage = "Failed to update user's reputation";
                return _result;
            }

            _result.IsSuccessful = true;

            return _result;
        }

        /// <summary>
        /// Calculates a user's new reputation when a new report is submitted
        /// </summary>
        /// <returns><see cref="UserProfile"/></returns>
        public async Task<DataResponse<UserProfile>> CalculateNewUserReputationAsync()
        {
            var userProfile = new DataResponse<UserProfile>();

            var getNewReputation = await _selectReports.SelectNewReputationAsync(_report).ConfigureAwait(false);

            var getOldReputation = await _selectUserProfile.SelectUserProfile(_userAccount.UserID);

            if (!getNewReputation.IsSuccessful)
            {
                _result.ErrorMessage = "Failed to retrieve user's ratings";
                return userProfile;
            }

            if (!getOldReputation.IsSuccessful)
            {
                _result.ErrorMessage = "Failed to retrieve user's current reputation";
                return userProfile;
            }

            _result.IsSuccessful = getOldReputation.IsSuccessful;

            var newRatings = getNewReputation.Data as Tuple<double, int>;

            double currentReputation = _userProfile.Reputation;
            double cumulativeRatings = (double)newRatings.Item1;
            double numberOfReports = (double)newRatings.Item2;

            double newReputation = (double)((currentReputation + cumulativeRatings) / (numberOfReports + 1));
            userProfile.Data = new UserProfile(_userProfile.UserID, newReputation, _userProfile.Identity.AuthenticationType);

            return userProfile;
        }

        public async Task<Response> StoreNewReportAsync()
        {

            var insertReport = await _insertReport.InsertUserReportAsync(_report).ConfigureAwait(false);

            if (!insertReport.IsSuccessful)
            {
                _result.ErrorMessage = "Failed to store report on user";
                return _result;
            }

            _result.IsSuccessful = true;

            return _result;
        }

        public async Task<Response> ResetUserReputation()
        {
            if (!((IPrincipal)_userProfile).IsInRole("Admin User"))
            {
                _result.ErrorMessage = "Unauthorized Request";
                return _result;
            }

            await _updateUserProfile.UpdateUserReputationAsync(_userProfile, 2.0).ConfigureAwait(false);

            return _result;
        }
    }
}