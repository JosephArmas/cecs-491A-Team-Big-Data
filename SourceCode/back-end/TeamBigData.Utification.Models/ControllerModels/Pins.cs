
namespace TeamBigData.Utification.Models.ControllerModels
{
    public class Pins
    {
        public int PinID { get; set; }
        public int UserID { get; set; }
        public String? Lat { get; set; }
        public String? Lng { get; set; }
        public int PinType { get; set; }
        public String? Description { get; set; }
        public String? Userhash { get; set; }
    }
}
