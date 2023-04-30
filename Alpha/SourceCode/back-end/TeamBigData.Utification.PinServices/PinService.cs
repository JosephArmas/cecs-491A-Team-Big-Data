﻿using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;
using System.Net.NetworkInformation;

namespace TeamBigData.Utification.PinServices
{
    public class PinService
    {
        private readonly DBConnectionString connectionString;
        public async Task<DataResponse<List<Pin>>> GetPinTable(string userHash)
        {
            List<Pin> pins = new List<Pin>();
            IDBSelecter sqlSelect = new SqlDAO(connectionString.devSqlFeatures);
            Log log;
            var logger = new Logger(new SqlDAO(connectionString.devSqlLogs));
            var result = await sqlSelect.SelectPinTable().ConfigureAwait(false);
            if (result.data.Count== 0)
            {
                log = new Log(1, "Error", userHash, "PinService.GetPinTable()", "Data", "Error Select Pint Table returns empty.");
                logger.Log(log);
            }
            else
            {
                log = new Log(1, "Info", userHash, "PinService.GetPinTable()", "Data", "Get pins table Successfully.");
                logger.Log(log);
            }
            return result;
        }
        public async Task<Response> StoreNewPin(Pin pin, string userHash)
        {
            IDBInserter sqlInsert = new SqlDAO(connectionString.devSqlFeatures);
            Log log = new Log(1, "info", userHash, "PinService.GetPinTable()", "Data", "Get pins table successfully.");
            var logger = new Logger(new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            var result = await sqlInsert.InsertPin(pin).ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                log = new Log(1, "Error", userHash, "PinService.StoreNewPin()", "Data", result.errorMessage);
                logger.Log(log);
            }
            else
            {
                log = new Log(1, "Info", userHash, "PinService.StoreNewPin()", "Data", result.errorMessage);
                logger.Log(log);
            }
            return result;
        }
        public async Task<Response> MarkAsCompleted(int pinID, string userHash)
        {
            IDBUpdater sqlUpdate = new SqlDAO(connectionString.devSqlFeatures);
            Log log = new Log(1, "info", userHash, "PinService.MarkAsCompleted()", "Data", "Mark pin as completed successfully.");
            var logger = new Logger(new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            var result = await sqlUpdate.UpdatePinToComplete(pinID);
            if (!result.isSuccessful)
            {
                log = new Log(1, "Error", userHash, "PinService.MarkAsCompleted()", "Data", result.errorMessage);
                logger.Log(log);
            }
            else
            {
                log = new Log(1, "Info", userHash, "PinService.MarkAsCompleted()", "Data", result.errorMessage);
                logger.Log(log);
            }
            return result;
        }

        public async Task<Response> ChangePinTypeTo(int pinID, int pinType, string userHash)
        {
            IDBUpdater sqlUpdate = new SqlDAO(connectionString.devSqlFeatures);
            Log log = new Log(1, "info", userHash, "PinService.ChangePinTypeTo()", "Data", "Change pin type successfully.");
            var logger = new Logger(new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
            var result = await sqlUpdate.UpdatePinType(pinID, pinType);
            if (!result.isSuccessful)
            {
                log = new Log(1, "Error", userHash, "PinService.ChangePinTypeTo()", "Data", result.errorMessage);
                logger.Log(log);
            }
            else
            {
                log = new Log(1, "Info", userHash, "PinService.ChangePinTypeTo()", "Data", result.errorMessage);
                logger.Log(log);
            }
            return result;
        }

        public async Task<Response> ChangePinContentTo(int pinID, string description, string userHash)
        {
            IDBUpdater sqlUpdate = new SqlDAO(connectionString.devSqlFeatures);
            Log log = new Log(1, "info", userHash, "PinService.ChangePinContentTo()", "Data", "Change pin content successfully.");
            var logger = new Logger(new SqlDAO(connectionString.devSqlLogs));
            var result = await sqlUpdate.UpdatePinContent(pinID, description);
            if (!result.isSuccessful)
            {
                log = new Log(1, "Error", userHash, "PinService.ChangePinContentTo()", "Data", result.errorMessage);
                logger.Log(log);
            }
            else
            {
                log = new Log(1, "Info", userHash, "PinService.ChangePinContentTo()", "Data", result.errorMessage);
                logger.Log(log);
            }
            return result;
        }

        public async Task<Response> DisablingPin(int pinID, string userHash)
        {
            IDBUpdater sqlUpdate = new SqlDAO(connectionString.devSqlFeatures);
            Log log = new Log(1, "info", userHash, "PinService.DisablingPin()", "Data", "Disabled pin successfully.");
            var logger = new Logger(new SqlDAO(connectionString.devSqlLogs));
            var result = await sqlUpdate.UpdatePinToDisabled(pinID);
            if (!result.isSuccessful)
            {
                log = new Log(1, "Error", userHash, "PinService.DisablingPin()", "Data", result.errorMessage);
                logger.Log(log);
            }
            else
            {
                log = new Log(1, "Info", userHash, "PinService.DisablingPin()", "Data", result.errorMessage);
                logger.Log(log);
            }
            return result;
        }
    }
}