using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.Models.ControllerModels
{
    public class Reports
    {
        public int UserID { get; set; }
        public double Rating { get; set; }
        public string Feedback { get; set; }
        public string CreateDate { get; set; }
        public int ReportingUserID { get; set; }
        public string ButtonCommand { get; set; }
        public int Partition { get; set; }
    }
}
