using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;
using System.Net.NetworkInformation;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Pins;
using TeamBigData.Utification.SQLDataAccess.DTO;

namespace TeamBigData.Utification.PinServices
{
    public class PinService
    {
        private readonly IPinDBInserter _pinDBInserter;
        private readonly IPinDBSelecter _pinDBSelecter;
        private readonly IPinDBUpdater _pinDBUpdater;

        public PinService(IPinDBInserter pinDBInserter, IPinDBSelecter pinDBSelecter, IPinDBUpdater pinDBUpdater)
        {
            _pinDBInserter = pinDBInserter;
            _pinDBSelecter = pinDBSelecter;
            _pinDBUpdater = pinDBUpdater;
        }

        public async Task<Response> StoreNewPin(Pin pin)
        {
            // TODO: Maybe log here and entry point

            var result = await _pinDBInserter.InsertNewPin(pin).ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                result.isSuccessful = false;
                result.errorMessage += ", {failed: _pinDBInserter.InsertNewPin}";
                return result;
            }
            else
            {
                result.isSuccessful = true;
            }

            return result;
        }

        public async Task<DataResponse<List<PinResponse>>> GetPinTable(string userHash)
        {
            // TODO: Maybe log here and entry point

            var result = await _pinDBSelecter.SelectPinTable().ConfigureAwait(false);
            if (!result.isSuccessful) 
            {
                result.isSuccessful = false;
                result.errorMessage += ", {failed: _pinDBSelecter.SelectPinTable}";
                return result;
            }
            else
            { 
                result.isSuccessful = true;
            }

            return result;
            /*List<Pin> pins = new List<Pin>();
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False";
            IDBSelecter sqlSelect = new SqlDAO(connectionString);
            Log log;
            var logger = new Logger(new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
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
            return result;*/
        }


        public async Task<Response> MarkAsCompleted(int pinID, int userID, String userHash)
        {
            // TODO: Maybe log here and entry point

            var result = await _pinDBUpdater.UpdatePinToComplete(pinID, userID).ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                result.isSuccessful = false;
                result.errorMessage += ", {false: _pinDBUpdater.UpdatePinToComplete}";
                return result;
            }
            else
            {
                result.isSuccessful = true;
            }

            return result;
            /*var connectionString = @"Server=.\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False";
            IDBUpdater sqlUpdate = new SqlDAO(connectionString);
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
            return result;*/
        }


        public async Task<Response> ChangePinContentTo(int pinID, int userID, String description, String userHash)
        {
            // TODO: Maybe log here and entry point

            var response = await _pinDBUpdater.UpdatePinContent(pinID, userID, description).ConfigureAwait(false);
            if (!response.isSuccessful)
            {
                response.isSuccessful = false;
                response.errorMessage += ", {failed: _pinDBUpdater.UpdatePinContent}";
                return response;
            }
            else 
            { 
                response.isSuccessful = true; 
            }

            return response;

            
            /*
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False";
            IDBUpdater sqlUpdate = new SqlDAO(connectionString);
            Log log = new Log(1, "info", userHash, "PinService.ChangePinContentTo()", "Data", "Change pin content successfully.");
            var logger = new Logger(new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
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
            */
        }

        public async Task<Response> ChangePinTypeTo(int pinID, int userID, int pinType, string userHash)
        {
            // TODO: Maybe log here and entry point

            var response = await _pinDBUpdater.UpdatePinType(pinID, userID, pinType).ConfigureAwait(false);
            if (!response.isSuccessful)
            {
                response.isSuccessful = false;
                response.errorMessage += ", {failed: _pinDBUpdater.UpdatePinType}";
                return response;
            }
            else
            {
                response.isSuccessful = true;
            }

            return response;

            /*
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False";
            IDBUpdater sqlUpdate = new SqlDAO(connectionString);
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
            */
        }

        public async Task<Response> DisablingPin(int pinID, int userID, string userHash)
        {
            // TODO: Maybe log here and entry point

            var response = await _pinDBUpdater.UpdatePinToDisabled(pinID, userID).ConfigureAwait(false);
            if (!response.isSuccessful)
            {
                response.isSuccessful = false;
                response.errorMessage += ", {failed: _pinDBUpdater.UpdatePinToDisabled}";
                return response;
            }
            else 
            { 
                response.isSuccessful = true; 
            }

            return response;

            /*
            var connectionString = @"Server=.\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False";
            IDBUpdater sqlUpdate = new SqlDAO(connectionString);
            Log log = new Log(1, "info", userHash, "PinService.DisablingPin()", "Data", "Disabled pin successfully.");
            var logger = new Logger(new SqlDAO(@"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True"));
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
            return result;*/
        }
    }
}