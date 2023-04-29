using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;
using System.Net.NetworkInformation;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Pins;
using TeamBigData.Utification.SQLDataAccess.DTO;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB;

namespace TeamBigData.Utification.PinServices
{
    public class PinService
    {
        private readonly IPinDBInserter _pinDBInserter;
        private readonly IPinDBSelecter _pinDBSelecter;
        private readonly IPinDBUpdater _pinDBUpdater;

        public PinService(PinsSqlDAO pinsSqlDAO)
        {
            _pinDBInserter = pinsSqlDAO;
            _pinDBSelecter = pinsSqlDAO;
            _pinDBUpdater = pinsSqlDAO;
        }

        public async Task<Response> StoreNewPin(Pin pin)
        {
            var response = await _pinDBInserter.InsertNewPin(pin).ConfigureAwait(false);

            if (!response.isSuccessful)
            {
                response.isSuccessful = false;
                response.errorMessage += ", {failed: _pinDBInserter.InsertNewPin}";
                return response;
            }
            else
            {
                response.isSuccessful = true;
            }

            return response;
        }

        public async Task<DataResponse<List<PinResponse>>> GetPinTable()
        {
            var pinResponse = await _pinDBSelecter.SelectPinTable().ConfigureAwait(false);

            if (!pinResponse.isSuccessful) 
            {
                pinResponse.isSuccessful = false;
                pinResponse.errorMessage += ", {failed: _pinDBSelecter.SelectPinTable}";
                return pinResponse;
            }
            else
            { 
                pinResponse.isSuccessful = true;
            }

            return pinResponse;
        }


        public async Task<Response> MarkAsCompleted(int pinID, int userID)
        {
            var response = await _pinDBUpdater.UpdatePinToComplete(pinID, userID).ConfigureAwait(false);

            if (!response.isSuccessful)
            {
                response.isSuccessful = false;
                response.errorMessage += ", {false: _pinDBUpdater.UpdatePinToComplete}";
                return response;
            }
            else
            {
                response.isSuccessful = true;
            }

            return response;
        }


        public async Task<Response> ChangePinContentTo(int pinID, int userID, String description)
        {
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
        }

        public async Task<Response> ChangePinTypeTo(int pinID, int userID, int pinType)
        {
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
        }

        public async Task<Response> DisablingPin(int pinID, int userID)
        {
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
        }
    }
}