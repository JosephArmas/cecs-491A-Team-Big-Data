using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Models.Abstraction;

namespace TeamBigData.Utification.Models
{
    public class UserProfile : IMyIPrincipal
    {
        public int _userID { get; private set; }
        public String _firstName { get; private set; }
        public String _lastName { get; private set; }
        public int _age { get; private set; }
        public String _address { get; private set; }
        public DateTime _birthday { get; private set; }
        public IIdentity? Identity { get; private set; }
        public UserProfile() 
        {
            _userID= 0;
            _firstName = "";
            _lastName = "";
            _age = 0;
            _address = "";
            _birthday = new DateTime();
            Identity = new GenericIdentity("0", "Anonymous User");
        }

        bool IPrincipal.IsInRole(string role)
        {
            if (this.Identity.AuthenticationType != role)
            {
                return false;
            }
            return true;
        }

        public UserProfile(int userID)
        {
            _userID= userID;
            _firstName = "";
            _lastName = "";
            _age = 0;
            _address = "";
            _birthday = new DateTime();
            Identity = new GenericIdentity(_userID.ToString(), "Anonymous User");
        }

        public UserProfile(int userID, string role)
        {
            _userID = userID;
            _firstName = "";
            _lastName = "";
            _age = 0;
            _address = "";
            _birthday = new DateTime();
            Identity = new GenericIdentity(_userID.ToString(), role);
        }

        public UserProfile(int userID, string firstName, string lastName, int age, string address, DateTime birthday, GenericIdentity identity)
        {
            _userID= userID;
            _firstName = firstName;
            _lastName = lastName;
            _age = age;
            _address = address;
            _birthday = birthday;
            Identity = identity;
        }
        public UserProfile(GenericIdentity identity)
        {
            this.Identity = identity;
        }
        public string ToString()
        {
            return ",   UserID: " + _userID + ",   Fullname: " + _firstName + " " + _lastName + ",   Age: " + _age + ",   Birthday: " + _birthday + ",   Role: " + Identity.AuthenticationType;
        }
    }
}
