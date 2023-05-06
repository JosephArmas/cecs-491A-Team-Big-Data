using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.Models.ControllerModels
{
    public class EventPin
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int UserID { get; set; }
        public int EventID { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
