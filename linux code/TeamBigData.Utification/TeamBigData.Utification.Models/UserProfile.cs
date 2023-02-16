﻿using System;
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
        public String _username { get; private set; }
        public String _firstName { get; private set; }
        public String _lastName { get; private set; }
        public int _age { get; private set; }
        public String _email { get; private set; }
        public String _address { get; private set; }
        public DateTime _birthday { get; private set; }
        public IIdentity? Identity { get; private set; }
        
        //mysql convert
        public String _cbirthday { get; private set; }

        bool IPrincipal.IsInRole(string role)
        {
            if (this.Identity.AuthenticationType != role)
            {
                return false;
            }
            return true;
        }
        public UserProfile(string username)
        {
            _username = username;
            _firstName = "";
            _lastName = "";
            _age = 0;
            _email = username;
            _address = "";
            _birthday = new DateTime();
            _cbirthday = _birthday.ToString("yyyy-MM-dd H:mm:ss");
            Identity = new GenericIdentity(username, "Anonymous User");
        }

        public UserProfile(string username, string role)
        {
            _username = username;
            _firstName = "";
            _lastName = "";
            _age = 0;
            _email = username;
            _address = "";
            _birthday = new DateTime();
            _cbirthday = _birthday.ToString("yyyy-MM-dd H:mm:ss");
            Identity = new GenericIdentity(username, role);
        }

        public UserProfile(string username, string firstName, string lastName, int age, string email, string address, DateTime birthday, GenericIdentity identity)
        {
            _username = username;
            _firstName = firstName;
            _lastName = lastName;
            _age = age;
            _email = email;
            _address = address;
            _birthday = birthday;
            _cbirthday = _birthday.ToString("yyyy-MM-dd H:mm:ss");
            Identity = identity;
        }
        public UserProfile(GenericIdentity identity)
        {
            this.Identity = identity;
        }
        public string ToString()
        {
            // for mysql convert bday
            // string _cbirthday = _birthday.ToString("yyyy-MM-dd H:mm:ss tt");
            return ",   Username: " + _username + ",   Fullname: " + _firstName + " " + _lastName + ",   Age: " + _age + ",   Birthday: " + _cbirthday + ",   Role: " + Identity.AuthenticationType;
        }
    }
}