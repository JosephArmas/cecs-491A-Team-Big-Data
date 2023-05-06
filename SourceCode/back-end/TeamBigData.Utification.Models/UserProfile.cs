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
        public String _address { get; private set; }
        public DateTime _birthday { get; private set; }
        public IIdentity? Identity { get; private set; }
        public UserProfile() 
        {
<<<<<<< HEAD
            Birthday = new DateTime(2000, 1, 1);
            Reputation = 2.0;
=======
            _userID= 0;
            _firstName = "";
            _lastName = "";
            _address = "";
            _birthday = new DateTime();
            _reputation = 2.0;
>>>>>>> parent of 7553d278 (Trying to integrate features together and fixing any merging problems)
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
<<<<<<< HEAD
            UserID = userID;
            Birthday = new DateTime(2000, 1, 1);
            Reputation = 2.0;
            Identity = new GenericIdentity(UserID.ToString(), "Anonymous User");
=======
            _userID= userID;
            _firstName = "";
            _lastName = "";
            _address = "";
            _birthday = new DateTime(2000, 1, 1);
            Identity = new GenericIdentity(_userID.ToString(), "Anonymous User");
        }

        public UserProfile(int userID, double reputation, string role)
        {
            _userID = userID;
            _firstName = "";
            _lastName = "";
            _address = "";
            _birthday = new DateTime();
            _reputation = reputation;
            Identity = new GenericIdentity(_userID.ToString(), role);
>>>>>>> parent of 7553d278 (Trying to integrate features together and fixing any merging problems)
        }

        public UserProfile(int userID, string role)
        {
<<<<<<< HEAD
            UserID = userID;
            Birthday = new DateTime(2000, 1, 1);
            Reputation = 2.0;
            Identity = new GenericIdentity(UserID.ToString(), role);
=======
            _userID = userID;
            _firstName = "";
            _lastName = "";
            _address = "";
            _birthday = new DateTime();
            _reputation = 2.0;
            Identity = new GenericIdentity(_userID.ToString(), role);
>>>>>>> parent of 7553d278 (Trying to integrate features together and fixing any merging problems)
        }
        public UserProfile(int userID, string firstName, string lastName, string address, DateTime birthday, GenericIdentity identity)
        {
            _userID = userID;
            _firstName = firstName;
            _lastName = lastName;
            _address = address;
            _birthday = birthday;
            Identity = identity;
        }
        public UserProfile(int userID, string firstName, string lastName, string address, DateTime birthday, double reputation, GenericIdentity identity)
        {
            _userID= userID;
            _firstName = firstName;
            _lastName = lastName;
            _address = address;
            _birthday = birthday;
            _reputation = reputation; 
            Identity = identity;
        }
        public UserProfile(GenericIdentity identity)
        {
            this.Identity = identity;
        }
        public string ToString()
        {
            return ",   UserID: " + _userID + ",   Fullname: " + _firstName + " " + _lastName + ",   Birthday: " + _birthday + ",   Reputation: " + _reputation + ",   Role: " + Identity.AuthenticationType;
        }
    }
}
