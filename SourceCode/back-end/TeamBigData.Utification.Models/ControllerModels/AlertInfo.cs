using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.Models.ControllerModels
{
    public class AlertInfo
    {
        public int AlertID { get; set; }
        public int UserID { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Description { get; set; }
        public int Read { get; set; }
        public string DateTime { get; set; }
        public string Zipcode { get; set; }
        public int PinType { get; set; }
    }
}
