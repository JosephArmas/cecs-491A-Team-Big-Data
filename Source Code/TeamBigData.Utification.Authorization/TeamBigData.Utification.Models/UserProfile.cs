using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Models.Abstraction;

namespace TeamBigData.Utification.Models
{
    public class UserProfile : MyIPrincipal
    {
        public string Fullname { get; set; }
        public int age { get; set; }
        public string Birthday { get; set; }
        public IIdentity Identity { get; private set; }
        public string Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string UserName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsInRole(string role) { if (role == Role) return true; return false; }
        public string Role { get; set; }

        public UserProfile() { }
        public UserProfile(string email)
        {
            this.Identity = new GenericIdentity(email);
            Role = "Anonymous User";
        }
    }
}
