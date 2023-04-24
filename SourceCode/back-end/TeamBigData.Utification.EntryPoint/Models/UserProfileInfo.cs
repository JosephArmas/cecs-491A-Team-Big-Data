using System.Security.Principal;

namespace Utification.EntryPoint.Models
{
    public class UserProfileInfo
    {
        public int UserID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public DateTime?  Birthday { get; set; }
        public double? Reputation { get; set; }
        public IIdentity? Identity { get; set; }
    }
}
