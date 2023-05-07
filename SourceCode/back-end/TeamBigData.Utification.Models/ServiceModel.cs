using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.Models
{
    public class ServiceModel
    {
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public string ServicePhone { get; set; }
        public string ServiceURL { get; set; }
        public int? ServiceID { get; set; }
        public string ServiceLat { get; set; }
        public string ServiceLong { get; set; }
        public int PinTypes { get; set; }
        public int Distance { get; set; }
        public int CreatedBy { get; set; }

    }
}
