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
        public string Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public int Age { get; set; }
        public string Birthday { get; set; }
        public bool ServiceProvider { get; set; }
        public IIdentity? Identity { get; private set; }
        bool IPrincipal.IsInRole(string role)
        {
            if (this.Identity.AuthenticationType != role)
            {
                return false;
            }
            return true;
        }
        public UserProfile(string userName)
        {
            this.Identity = new GenericIdentity(userName, "Anonymous User");
        }
        public UserProfile(GenericIdentity identity)
        {
            this.Identity = identity;
        }
        public string ToString()
        {
            return "ID: " + Id + ",   Username: " + Username + ",   Fullname: " + Fullname + ",   Age: " + Age + ",   Birthday: " + Birthday + ",   Role: " + Identity.AuthenticationType ;
        }
    }
}
