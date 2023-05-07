using System.ComponentModel.DataAnnotations.Schema;

namespace TeamBigData.Utification.Models
{
    public class Pin
    {
        public int PinID { get; set; }
        public int UserID { get; set; }
        public String Lat { get; set; }
        public String Lng { get; set; }
        public int PinType { get; set; }
        public String Description { get; set; }
        public int Disabled { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastModified { get; set; }
        public int UserLastModified { get; set; }

        public Pin()
        {
            DateCreated = new DateTime(2000,1,1);
            DateLastModified = new DateTime(2000, 1, 1);
        }

        public Pin(int pinID, int userID, String lat, String lng, int pinType, String description, int disabled, DateTime dateCreated, DateTime dateLastModified, int userLastModified)
        {
            PinID = pinID;
            UserID = userID;
            Lat = lat;
            Lng = lng;
            PinType = pinType;
            Description = description;
            Disabled = disabled;
            DateCreated = dateCreated;
            DateLastModified = dateLastModified;
            UserLastModified = userLastModified;
        }
        public Pin(int pinID, int userID, String lat, String lng, int pinType, String description, int disabled, DateTime dateCreated)
        {
            PinID = pinID;
            UserID = userID;
            Lat = lat;
            Lng = lng;
            PinType = pinType;
            Description = description;
            Disabled = disabled;
            DateCreated = dateCreated;
            DateLastModified = new DateTime(2000, 1, 1);
        }
        public Pin(int userID, string lat, string lng, int pinType, string description)
        {
            UserID = userID;
            Lat = lat;
            Lng = lng;
            PinType = pinType;
            Description = description;
            DateCreated = new DateTime(2000, 1, 1);
            DateLastModified = new DateTime(2000, 1, 1);
        }

        public string ToString()
        {
            return $"{{ PinID: {PinID}, UserID: {UserID}, Lat: {Lat}, Lng: {Lng}, PinType: {PinType}, Description: {Description}, Disabled: {Disabled}, DateCreated: {DateCreated}, DateLastModified: {DateLastModified}, UserLastModified: {UserLastModified} }}";

        }
    }
}
