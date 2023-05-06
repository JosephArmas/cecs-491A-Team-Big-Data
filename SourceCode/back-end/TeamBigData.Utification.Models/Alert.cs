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
        public int _alertID { get; set; }
        public int _userID { get; set; }
        //6decimal 
        public string _lat { get; set; }
        public string _lng { get; set; }
        public int _pinType { get; set; }
        public string _description { get; set; }
        public int _read { get; set; }
        public string _dateTime { get; set; }
        public string _zipcode { get; set; }


        public Alert(int alertID, int userID, string lat, string lng, int pintype, string description, int read, string dateTime, string zipcode)
        {
            _alertID = alertID;
            _userID = userID;
            _lat = lat;
            _lng = lng;
            _pinType = pintype;
            _description = description;
            _read = read;
            _dateTime = dateTime;
            _zipcode = zipcode;
        }
        public Alert(int alertID, string lat, string lng, string description, int read, string dateTime, string zipcode)
        {
            _alertID = alertID;
            _userID = 0;
            _lat = lat;
            _lng = lng;
            _pinType = 0;
            _description = description;
            _read = read;
            _dateTime = dateTime;
            _zipcode = zipcode;
        }


        public Alert(int userID, string lat, string lng, string description, string dateTime, string zipcode)
        {
            _alertID = 0;
            _userID = userID;
            _lat = lat;
            _lng = lng;
            _pinType = 0;
            _description = description;
            _read = 0;
            _dateTime = dateTime;
            _zipcode = zipcode;
        }

        public Alert(int alertID, int userID, string lat, string lng, int pinType, string description, int read, string dateTime)
        {
            _alertID = alertID;
            _userID = userID;
            _lat = lat;
            _lng = lng;
            _pinType = pinType;
            _description = description;
            _read = read;
            _dateTime = dateTime;
            _zipcode = "";
        }

        public Alert(int userID, string lat, string lng, string description)
        {
            _alertID = 0;
            _userID = userID;
            _lat = lat;
            _lng = lng;
            _pinType = 0;
            _description = description;
            _read = 0;
            _dateTime = DateTime.Now.ToString(); ;
            _zipcode = "";
        }
        public string ToString()
        {
            return "{ _alertID: " + _alertID + ", _userID: " + _userID + ", _lat: " + _lat + ", _lng: " + _lng + "_description: " + _description + ", read: " + _read + ", dateTime: " + _dateTime + ", zipcode: " + _zipcode + "}";
        }
    }

}