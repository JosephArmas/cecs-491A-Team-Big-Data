﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.SQLDataAccess.DTO
{
    public class PinResponse
    {
        public int _pinID { get; set; }
        public int _userID { get; set; }
        public String _lat { get; set; }
        public String _lng { get; set; }
        public int _pinType { get; set; }
        public String _description { get; set; }
        public int _disabled { get; set; }
        public int _completed { get; set; }
        public String _dateCreated { get; set; }

        public PinResponse(int pinID, int userID, String lat, String lng, int pinType, String description, int disabled, int completed, String dateCreated)
        {
            _pinID = pinID;
            _userID = userID;
            _lat = lat;
            _lng = lng;
            _pinType = pinType;
            _description = description;
            _disabled = disabled;
            _completed = completed;
            _dateCreated = dateCreated;
        }
    }
}