using System;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.AlertServices;
using TeamBigData.Utification.ErrorResponse;
using System.Reflection.Metadata;
using System.Security.Claims;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.SQLDataAccess;

namespace TeamBigData.Utification.AlertManagers
{
    public class AlertManager
    {
        private readonly AlertService _alertService;
        /* public AlertManager(AlertService alertService)
         {
             _alertService = alertService;
         }*/
        //private readonly string _logConnectionString = @"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True";
        //private readonly string _logs = @"LogsSQLDBConnection";
        /*public async Task<DataResponse<List<Alert>>> GetListOfAllAlerts(string userHash)
        {
            AlertService alertSer = new AlertService();
            List<Alert> alert = await alertSer.GetAlertTable(userHash);
            return alert;
        }*/
        public async Task<DataResponse<List<Alert>>> GetListOfAllAlerts(String userHash)
        {
            // TODO: Check if process time follows business rules

            var result = await _alertService.GetAlertTable(userHash).ConfigureAwait(false);
            if (!result.IsSuccessful)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Failed to get Alert Table";
                return result;
            }
            else
            {
                result.IsSuccessful = true;
            }

            return result;
        }
        /*public async Task<int[]> GetAlertsAdded()
        {
            IDBAlert alerts = new SqlDAO(_logConnectionString);
            int[] rows = new int[100];
            var response = await alerts.GetAlertsAdded(ref rows);
            return rows;
        }*/
        public async Task<Response> SaveNewAlert(Alert alert, string userHash)
        {

            var result = await _alertService.StoreNewAlert(alert, userHash).ConfigureAwait(false);
            if (!result.IsSuccessful)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Failed to save Alert";
                return result;
            }
            else
            {
                result.IsSuccessful = true;
            }

            return result;
        }
        /*public Response SaveNewAlert(Alert alert, string userHash)
        {
            AlertService alertSer = new AlertService();
            Response response = alertSer.StoreNewAlert(alert, userHash).Result;
            return response;
        }*/

        /*public Response MarkAsRead(int alertID,int userID, string userHash)
        {
            AlertService alertSer = new AlertService();
            Response response = alertSer.MarkAsRead(alertID, userID, userHash).Result;
            return response;
        }*/
        public async Task<Response> MarkAsRead(int alertID, int userID, String userHash)
        {
            var result = await _alertService.MarkAsRead(alertID, userID, userHash).ConfigureAwait(false);
            if (!result.IsSuccessful)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Failed to Mark alert as read";
                return result;
            }
            else
            {
                result.IsSuccessful = true;
            }

            return result;
        }
        /*public async Task<Response> ModifyAlert(int alertID, string description, string userHash)
        {
            AlertService alertSer = new AlertService();
            Response response = await alertSer.ModifyAlert(alertID, description, userHash);
            return response;
        }*/
        public async Task<Response> ModifyAlert(int alertID, int userID, String description, String userHash)
        {
            // TODO: Check if process time follows business rules

            var response = await _alertService.ModifyAlert(alertID, userID, description, userHash).ConfigureAwait(false);
            if (!response.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Failed to Modify Alert description";
            }
            else
            {
                response.IsSuccessful = true;
            }

            return response;
        }
    }
}