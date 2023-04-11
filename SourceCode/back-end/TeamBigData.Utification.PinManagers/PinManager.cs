using TeamBigData.Utification.Models;
using TeamBigData.Utification.PinServices;
using TeamBigData.Utification.ErrorResponse;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace TeamBigData.Utification.PinManagers
{
    public class PinManager
    {
        public async Task<DataResponse<List<Pin>>> GetListOfAllPins(string userHash)
        {
            PinService pinSer = new PinService();
            var pins = await pinSer.GetPinTable(userHash);
            return pins;
        }

        public async Task<Response> SaveNewPin(Pin newPin, string userHash)
        {
            PinService pinSer = new PinService();
            Response response = await pinSer.StoreNewPin(newPin, userHash);
            return response;
        }

        public async Task<Response> MarkAsCompletedPin(int pinID, string userHash)
        {
            PinService pinSer = new PinService();
            Response response = await pinSer.MarkAsCompleted(pinID, userHash);
            return response;
        }

        public async Task<Response> ChangePinType(int pinID, int pinType, string userHash) {
            PinService pinSer = new PinService();
            Response response = await pinSer.ChangePinTypeTo(pinID, pinType, userHash);
            return response;
        }

        public async Task<Response> ChangePinContent(int pinID, string description, string userHash)
        {
            PinService pinSer = new PinService();
            Response response = await pinSer.ChangePinContentTo(pinID, description, userHash);
            return response;
        }

        public async Task<Response> DisablePin(int pinID, string userHash)
        {
            PinService pinSer = new PinService();
            Response response = await pinSer.DisablingPin(pinID, userHash);
            return response;
        }

    }
}