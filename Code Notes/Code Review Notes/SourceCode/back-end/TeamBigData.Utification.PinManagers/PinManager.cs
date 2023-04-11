using TeamBigData.Utification.Models;
using TeamBigData.Utification.PinServices;
using TeamBigData.Utification.ErrorResponse;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace TeamBigData.Utification.PinManagers
{
    //acts as pass through objects since its not doing anything just passing through
    //provides no value
    //value:
    //check for inputs
    //business rules
    public class PinManager
    {
        public List<Pin> GetListOfAllPins(string userHash)
        {
            //not using dependicy injection na d depedencies are hardcoded
            //hard coding makes it hard to know changes done
            PinService pinSer = new PinService();
            //should not put .Result since its waiting for a response so theres no asynchronus value
            //blocks threads to wait for this value
            //.Result makes code perform result
            List<Pin> pins = pinSer.GetPinTable(userHash).Result;
            //before returning pins evaluate response
            //if not valid then why return back not valid value
            //handles error situations
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