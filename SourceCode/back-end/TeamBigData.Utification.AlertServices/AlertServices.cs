using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;
using System.Net.NetworkInformation;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Alerts;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB;
using TeamBigData.Utification.Logging.Abstraction;

namespace TeamBigData.Utification.AlertServices
{
    public class AlertService
    {
        private readonly IAlertDBInserter _Inserter;
        private readonly IAlertDBSelecter _Selecter;
        private readonly IAlertDBUpdater _Updater;
        //private readonly IDBAlert _DBAlert;
        private readonly ILogger _logger;

        public AlertService(AlertsSqlDAO alertsSqlDAO, ILogger logger)
        {
            _Inserter = alertsSqlDAO;
            _Selecter = alertsSqlDAO;
            _Updater = alertsSqlDAO;
            _logger = logger;
            //_DBAlert = DBAlert;
        }

        public async Task<DataResponse<List<Alert>>> GetAlertTable(string userHash)
        {
            //var logger = new Logger(new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            await _logger.Logs(new Log(0, "Info", userHash, "Get List Of All Alerts", "Data", "User is attempting to get list of all alerts (Services)"));
            var result = await _Selecter.SelectAlertTable().ConfigureAwait(false);
            if (!result.IsSuccessful)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Failed to retrieved Alerts";
                await _logger.Logs(new Log(0, "Error", userHash, "_alertService.GetAlertTable", "Data", "Failed to get alerts(Services)."));
                //Log log = new Log(1, "Error", userHash, "AlertService.GetAlertTable()", "Data", "Failed to retrieved Alerts");
                //await logger.Log(log);
                return result;
            }
            else
            {
                result.IsSuccessful = true;
                Log log = new Log(1, "Info", userHash, "AlertService.GetAlertTable()", "Data", "Alerts retrieved");
                //await logger.Log(log);
            }
            
            return result;
        }
        public async Task<Response> StoreNewAlert(Alert alert, string userHash)
        {
            //var logger = new Logger(new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            Log log = new Log(1, "info", userHash, "AlertService.GetAlertTable()", "Data", "Get alert table successfully.");
            var result = await _Inserter.InsertAlert(alert).ConfigureAwait(false);
            if (!result.IsSuccessful)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Alert failed to insert";
                log = new Log(1, "Error", userHash, "AlertService.StoreNewAlert()", "Data", result.ErrorMessage);
                //await logger.Log(log);

                return result;
            }
            else
            {
                log = new Log(1, "Info", userHash, "AlertService.StoreNewAlert()", "Data", result.ErrorMessage);
                //await logger.Log(log);
                result.IsSuccessful = true;
            }

            return result;
        }
        public async Task<Response> MarkAsRead(int alertID, int userID, String userHash)
        {
            Log log = new Log(1, "info", userHash, "AlertService.MarkAsRead()", "Data", "Mark alert as read successfully.");
            //var logger = new Logger(new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            var result = await _Updater.MarkAsRead(alertID, userID).ConfigureAwait(false);
            if (!result.IsSuccessful)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Failed to mark alert as read";
                log = new Log(1, "Error", userHash, "AlertService.MarkAsRead()", "Data", result.ErrorMessage);
                //await logger.Log(log);
                return result;
            }
            else
            {
                result.IsSuccessful = true;
                log = new Log(1, "Info", userHash, "AlertService.MarkAsRead()", "Data", result.ErrorMessage);
                // await logger.Log(log);
            }

            return result;
        }
        public async Task<Response> ModifyAlert(int alertID, int userID, String description, String userHash)
        {
            Log log = new Log(1, "info", userHash, "AlertService.ModifyAlert()", "Data", "Mark modify alert as successfull.");
            //var logger = new Logger(new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            var result = await _Updater.UpdateAlertContent(alertID, userID, description).ConfigureAwait(false);
            if (!result.IsSuccessful)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Failed to modify alert";
                log = new Log(1, "Error", userHash, "AlertService.ModifyAlert()", "Data", result.ErrorMessage);
                //await logger.Log(log);
                return result;
            }
            else
            {
                result.IsSuccessful = true;
                log = new Log(1, "Info", userHash, "AlertService.ModifyAlert()", "Data", result.ErrorMessage);
                //await logger.Log(log);
            }

            return result;
        }
    }
}