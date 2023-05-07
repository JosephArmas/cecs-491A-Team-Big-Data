using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;
using System.Reflection.Emit;

namespace TeamBigData.Utification.Models
{
    public class Alert
    {
        public int AlertID { get; set; }
        public int UserID { get; set; }
        //6decimal 
        public string Lat { get; set; }
        public string Lng { get; set; }
        public int PinType { get; set; }
        public string Description { get; set; }
        public int Read { get; set; }
        public string DateTime { get; set; }
        public string Zipcode { get; set; }


        public Alert(int alertID, int userID, string lat, string lng, int pintype, string description, int read, string dateTime, string zipcode)
        {
            AlertID = alertID;
            UserID = userID;
            Lat = lat;
            Lng = lng;
            PinType = pintype;
            Description = description;
            Read = read;
            DateTime = dateTime;
            Zipcode = zipcode;
        }
        public Alert(int alertID, string lat, string lng, string description, int read, string dateTime, string zipcode)
        {
            AlertID = alertID;
            UserID = 0;
            Lat = lat;
            Lng = lng;
            PinType = 0;
            Description = description;
            Read = read;
            DateTime = dateTime;
            Zipcode = zipcode;
        }


        public Alert(int userID, string lat, string lng, string description, string dateTime, string zipcode)
        {
            AlertID = 0;
            UserID = userID;
            Lat = lat;
            Lng = lng;
            PinType = 0;
            Description = description;
            Read = 0;
            DateTime = dateTime;
            Zipcode = zipcode;
        }

        public Alert(int alertID, int userID, string lat, string lng, int pinType, string description, int read, string dateTime)
        {
            AlertID = alertID;
            UserID = userID;
            Lat = lat;
            Lng = lng;
            PinType = pinType;
            Description = description;
            Read = read;
            DateTime = dateTime;
            Zipcode = "";
        }

        public Alert(int userID, string lat, string lng, string description)
        {
            AlertID = 0;
            UserID = userID;
            Lat = lat;
            Lng = lng;
            PinType = 0;
            Description = description;
            Read = 0;
            DateTime = System.DateTime.UtcNow.ToString();
            Zipcode = "";
        }
        public string ToString()
        {
            return "{ _alertID: " + AlertID + ", _userID: " + UserID + ", _lat: " + Lat + ", _lng: " + Lng + "_description: " + Description + ", read: " + Read + ", dateTime: " + DateTime + ", zipcode: " + Zipcode + "}";
        }
    }

}