using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.Models
{
    public class RequestModel
    {
        public int RequestID { get; set; }
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public int Requester { get; set; }
        public string RequestLat { get; set; }
        public string RequestLong { get; set; }
        public int PinType { get; set; }
        public int Accept { get; set; }
        public int? Distance { get; set; }
    }
}
