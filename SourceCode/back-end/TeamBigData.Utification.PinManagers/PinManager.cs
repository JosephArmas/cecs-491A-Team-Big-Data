using TeamBigData.Utification.Models;
using TeamBigData.Utification.PinServices;
using TeamBigData.Utification.ErrorResponse;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace TeamBigData.Utification.PinManagers
{
    public class PinManager
    {
        public List<Pin> GetListOfAllPins(string userHash)
        {
            PinService pinSer = new PinService();
            List<Pin> pins = pinSer.GetPinTable(userHash).Result;
            return pins;
        }

        public Response SaveNewPin(Pin newPin, string userHash)
        {
            PinService pinSer = new PinService();
            Response response = pinSer.StoreNewPin(newPin, userHash).Result;
            return response;
        }

        public Response MarkAsCompletedPin(int pinID, string userHash)
        {
            PinService pinSer = new PinService();
            Response response = pinSer.MarkAsCompleted(pinID, userHash).Result;
            return response;
        }

        public Response ChangePinType(int pinID, int pinType, string userHash) {
            PinService pinSer = new PinService();
            Response response = pinSer.ChangePinTypeTo(pinID, pinType, userHash).Result;
            return response;
        }

        public Response ChangePinContent(int pinID, string description, string userHash)
        {
            PinService pinSer = new PinService();
            Response response = pinSer.ChangePinContentTo(pinID, description, userHash).Result;
            return response;
        }

        public Response DisablePin(int pinID, string userHash)
        {
            PinService pinSer = new PinService();
            Response response = pinSer.DisablingPin(pinID, userHash).Result;
            return response;
        }

    }
}