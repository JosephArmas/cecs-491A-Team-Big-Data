using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Models.Abstraction;

namespace TeamBigData.Utification.Models
{
    public class UserProfile : IMyIPrincipal
    {
        public int UserID { get; private set; }
        public String FirstName { get; private set; }
        public String LastName { get; private set; }
        public String Address { get; private set; }
        public DateTime Birthday { get; private set; }
        public double Reputation { get; private set; }
        public int PinsCompleted { get; private set; }
        public IIdentity? Identity { get; private set; }
        public UserProfile() 
        {
            Birthday = new DateTime(2000, 1, 1);
            Reputation = 2.0;
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
            UserID = userID;
            Birthday = new DateTime(2000, 1, 1);
            Reputation = 2.0;
            Identity = new GenericIdentity(UserID.ToString(), "Anonymous User");
        }

        public UserProfile(int userID, string role)
        {
            UserID = userID;
            Birthday = new DateTime(2000, 1, 1);
            Reputation = 2.0;
            Identity = new GenericIdentity(UserID.ToString(), role);
        }

        public UserProfile(int userID, string firstName, string lastName, string address, DateTime birthday, double reputation, GenericIdentity identity)
        {
            UserID = userID;
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            Birthday = birthday;
            Reputation=reputation;
            Identity = identity;
        }

        public UserProfile(int userID, string firstName, string lastName, string address, DateTime birthday, double reputation, int pinsCompleted, GenericIdentity identity)
        {
            UserID = userID;
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            Birthday = birthday;
            Reputation = reputation;
            PinsCompleted = pinsCompleted;
            Identity = identity;
        }

        public UserProfile(int userID, double reputation, string role)
        {
            UserID = userID;
            Reputation = reputation;
            Identity = new GenericIdentity(UserID.ToString(), role);
        }
        public UserProfile(GenericIdentity identity)
        {
            Identity = identity;
        }

        public string ToString()
        {
            return ",   UserID: " + UserID + ",   Fullname: " + FirstName + " " + LastName + ",   Birthday: " + Birthday + ", Reputation: "+ Reputation +",  Role: " + Identity.AuthenticationType;
        }
    }
}
