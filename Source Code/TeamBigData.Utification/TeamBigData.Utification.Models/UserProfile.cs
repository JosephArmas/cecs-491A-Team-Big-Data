using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.AccountServices
{
    public class UserProfile
    {
        private String _username;
        private String _firstName;
        private String _lastName;
        private String _email;
        private String _address;
        private String _birthday;

        public UserProfile(string username)
        {
            _username = username;
            _firstName = "";
            _lastName = "";
            _email = username;
            _address = "";
            _birthday = "";
        }

        public UserProfile(string username, string firstName, string lastName, string email, string address, string birthday)
        {
            _username = username;
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _address = address;
            _birthday = birthday;
        }

        public String GetUsername()
        {
            return _username;
        }

        public String GetFirstName()
        {
            return _firstName;
        }

        public String GetLastName()
        {
            return _lastName;
        }

        public String GetEmail()
        {
            return _email;
        }

        public String GetAddress()
        {
            return _address;
        }

        public String GetBirthday()
        {
            return _birthday;
        }

        public void SetUsername(String username)
        {
            _username = username;
        }

        public void SetFirstName(String firstName)
        {
            _firstName = firstName;
        }

        public void SetLastName(String lastName)
        {
            _lastName = lastName;
        }

        public void SetEmail(String email)
        {
            _email = email;
        }

        public void SetAddress(String address)
        {
            _address = address;
        }

        public void SetBirthday(String birthday)
        {
            _birthday = birthday;
        }
    }
}
