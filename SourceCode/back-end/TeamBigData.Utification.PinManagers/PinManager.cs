using TeamBigData.Utification.Models;
using TeamBigData.Utification.PinServices;
using TeamBigData.Utification.ErrorResponse;
namespace TeamBigData.Utification.PinManagers
{
    public class PinManager
    {
        public List<Pin> GetListOfAllPins(UserAccount userAccount)
        {
            PinService pinSer = new PinService();
            List<Pin> pins = pinSer.GetPinTable(userAccount).Result;
            return pins;
        }

        public Response SaveNewPin(Pin newPin, UserAccount userAccount)
        {
            PinService pinSer = new PinService();
            Response response = pinSer.StoreNewPin(newPin, userAccount).Result;
            return response;
        }
    }
}