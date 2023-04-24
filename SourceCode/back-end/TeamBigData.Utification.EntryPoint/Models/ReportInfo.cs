using Microsoft.AspNetCore.Mvc;

namespace Utification.EntryPoint.Models
{
    [BindProperties]
    public class ReportInfo
    {        
        public double Rating { get; set; }
        public int ReportedUser { get; set; }
        public int ReportingUser { get; set; }
        public string Feedback { get; set; }
    }
}
