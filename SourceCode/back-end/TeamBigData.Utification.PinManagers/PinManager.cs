using TeamBigData.Utification.Models;
using TeamBigData.Utification.PinServices;
using TeamBigData.Utification.ErrorResponse;
using System.Reflection.Metadata;
using System.Security.Claims;
using TeamBigData.Utification.SQLDataAccess.DTO;

namespace TeamBigData.Utification.PinManagers
{
    public class PinManager
    {
        private readonly PinService _pinService;
        public PinManager(PinService pinService) 
        { 
            _pinService = pinService;
        }
        public async Task<Response> SaveNewPin(Pin pin)
        {
            // TODO: Check if process time follows business rules

            var result = await _pinService.StoreNewPin(pin).ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                result.isSuccessful = false;
                result.errorMessage += ", {false: _pinService.StoreNewPin}";
                return result;
            }
            else
            {
                result.isSuccessful = true;
            }

            return result;
        }

        public async Task<DataResponse<List<PinResponse>>> GetListOfAllPins(String userHash)
        {
            // TODO: Check if process time follows business rules

            var result = await _pinService.GetPinTable("").ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                result.isSuccessful = false;
                result.errorMessage += ", {failed: _pinService.GetPinTable}";
                return result;
            }
            else
            {
                result.isSuccessful = true;
            }

            return result;
        }

        public async Task<Response> MarkAsCompletedPin(int pinID, int userID, String userHash)
        {
            // TODO: Check if process time follows business rules

            var result = await _pinService.MarkAsCompleted(pinID, userID, userHash).ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                result.isSuccessful = false;
                result.errorMessage += ", {failed: _pinService.MarkAsCompleted}";
                return result;
            }
            else
            { 
                result.isSuccessful = true; 
            }

            return result;
        }

        public async Task<Response> ChangePinContent(int pinID, int userID, String description, String userHash)
        {
            // TODO: Check if process time follows business rules

            var response = await _pinService.ChangePinContentTo(pinID, userID, description, userHash).ConfigureAwait(false);
            if (!response.isSuccessful)
            {
                response.isSuccessful = false;
                response.errorMessage += ", {failed: _pinService.ChangePinContentTo}";
            }
            else 
            {
                response.isSuccessful = true;
            }

            return response;
        }
        public async Task<Response> ChangePinType(int pinID, int userID, int pinType, String userHash)
        {
            // TODO: Check if process time follows business rules

            var response = await _pinService.ChangePinTypeTo(pinID, userID, pinType, userHash).ConfigureAwait(false);
            if (!response.isSuccessful)
            {
                response.isSuccessful = false;
                response.errorMessage += ", {failed: _pinService.ChangePinTypeTo}";
                return response;
            }
            else
            { 
                response.isSuccessful = true; 
            }

            return response;
        }

        
        public async Task<Response> DisablePin(int pinID, int userID, string userHash)
        {
            // TODO: Check if process time follows business rules

            var response = await _pinService.DisablingPin(pinID, userID, userHash).ConfigureAwait(false);
            if (!response.isSuccessful)
            {
                response.isSuccessful = false;
                response.errorMessage += ", {failed: _pinService.DisablingPin}";
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