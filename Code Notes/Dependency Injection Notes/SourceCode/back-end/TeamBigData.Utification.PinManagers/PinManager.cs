using TeamBigData.Utification.Models;
using TeamBigData.Utification.Models.DTO;
using TeamBigData.Utification.PinServices;
using TeamBigData.Utification.ErrorResponse;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace TeamBigData.Utification.PinManagers
{
    public class PinManager
    {
        private readonly PinService _pinService;
        public PinManager(PinService pinService) 
        {
            _pinService = pinService;
        }
        public async Task<List<Pin>> GetListOfAllPins(string userHash)
        {
            //PinService pinSer = new PinService();
            var pins = await _pinService.GetPinTable(userHash).ConfigureAwait(false);
            return pins;
        }

        public async Task<Response> SaveNewPin(CreatePinDto newPin, string userHash)
        {
            //PinService pinSer = new PinService();
            Response response = await _pinService.StoreNewPin(newPin, userHash).ConfigureAwait(false);
            return response;
        }

        public Response MarkAsCompletedPin(int pinID, string userHash)
        {
            //PinService pinSer = new PinService();
            Response response = _pinService.MarkAsCompleted(pinID, userHash).Result;
            return response;
        }

        public Response ChangePinType(int pinID, int pinType, string userHash) {
            //PinService pinSer = new PinService();
            Response response = _pinService.ChangePinTypeTo(pinID, pinType, userHash).Result;
            return response;
        }

        public Response ChangePinContent(int pinID, string description, string userHash)
        {
            //PinService pinSer = new PinService();
            Response response = _pinService.ChangePinContentTo(pinID, description, userHash).Result;
            return response;
        }

        public Response DisablePin(int pinID, string userHash)
        {
            //PinService pinSer = new PinService();
            Response response = _pinService.DisablingPin(pinID, userHash).Result;
            return response;
        }

    }
}