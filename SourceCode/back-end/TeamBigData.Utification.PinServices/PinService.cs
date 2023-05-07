using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;
using System.Net.NetworkInformation;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Pins;
using TeamBigData.Utification.SQLDataAccess.DTO;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB;
using Microsoft.IdentityModel.Tokens;

namespace TeamBigData.Utification.PinServices
{
    public class PinService
    {
        private readonly IPinDBInserter _pinDBInserter;
        private readonly IPinDBSelecter _pinDBSelecter;
        private readonly IPinDBUpdater _pinDBUpdater;
        private readonly IPinDBDeleter _pinDBDeleter;

        public PinService(PinsSqlDAO pinsSqlDAO)
        {
            _pinDBInserter = pinsSqlDAO;
            _pinDBSelecter = pinsSqlDAO;
            _pinDBUpdater = pinsSqlDAO;
            _pinDBDeleter = pinsSqlDAO;
        }

        public async Task<Response> StoreNewPin(Pin pin)
        {
            var response = await _pinDBInserter.InsertNewPin(pin).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.ErrorMessage += ", {failed: _pinDBInserter.InsertNewPin}";
                return response;
            }
            else
            {
                response.IsSuccessful = true;
            }

            return response;
        }

        public async Task<DataResponse<List<PinResponse>>> GetPinTable()
        {
            var pinResponse = await _pinDBSelecter.SelectEnabledPins().ConfigureAwait(false);

            if (!pinResponse.IsSuccessful) 
            {
                pinResponse.IsSuccessful = false;
                pinResponse.ErrorMessage += ", {failed: _pinDBSelecter.SelectPinTable}";
            }
            else if (pinResponse.Data.IsNullOrEmpty())
            {
                pinResponse.IsSuccessful = true;
                pinResponse.ErrorMessage = "Returning Empty List of Pins";
            }
            else 
            { 
                pinResponse.IsSuccessful = true;
                pinResponse.ErrorMessage = "Returning List of Pins.";
            }


            return pinResponse;
        }


        public async Task<Response> DeletePin(int pinID)
        {
            // TODO: Delete other linking features to this pin
            var response = await _pinDBDeleter.DeletePinFromTable(pinID).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.ErrorMessage += ", {false: _pinDBUpdater.UpdatePinToComplete}";
                return response;
            }
            else
            {
                response.IsSuccessful = true;
            }

            return response;
        }


        public async Task<Response> ChangePinContentTo(int pinID, int userID, String description)
        {
            var response = await _pinDBUpdater.UpdatePinContent(pinID, userID, description).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.ErrorMessage += ", {failed: _pinDBUpdater.UpdatePinContent}";
                return response;
            }
            else 
            { 
                response.IsSuccessful = true; 
            }

            return response;
        }

        public async Task<Response> ChangePinTypeTo(int pinID, int userID, int pinType)
        {
            var response = await _pinDBUpdater.UpdatePinType(pinID, userID, pinType).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.ErrorMessage += ", {failed: _pinDBUpdater.UpdatePinType}";
                return response;
            }
            else
            {
                response.IsSuccessful = true;
            }

            return response;
        }

        public async Task<Response> DisablingPin(int pinID, int userID)
        {
            var response = await _pinDBUpdater.UpdatePinToDisabled(pinID, userID).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.ErrorMessage += ", {failed: _pinDBUpdater.UpdatePinToDisabled}";
                return response;
            }
            else 
            { 
                response.IsSuccessful = true; 
            }

            return response;
        }
    }
}