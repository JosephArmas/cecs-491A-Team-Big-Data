using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.SQLDataAccess.DTO
{
    public class PinResponse
    {
        public int PinID { get; set; }
        public int UserID { get; set; }
        public String Lat { get; set; }
        public String Lng { get; set; }
        public int PinType { get; set; }
        public String Description { get; set; }
        public String DateCreated { get; set; }

        public PinResponse(int pinID, int userID, String lat, String lng, int pinType, String description, String dateCreated) 
        {
            PinID = pinID;
            UserID = userID;
            Lat = lat;
            Lng = lng;
            PinType = pinType;
            Description = description;
            DateCreated = dateCreated;
        }
    }
}
