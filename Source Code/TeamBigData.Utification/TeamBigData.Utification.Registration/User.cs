using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TeamBigData.Utification.Registration
{
    public class User
    {
        private String? username;
        private String? password;
        private String? email;
        public User()
        {

        }

        public User(String username, String password, String email)
        {
            if (User.IsValidUsername(username) && User.IsValidPassword(password) && User.IsValidEmail(email))
            {
                this.username = username;
                this.password = password;
                this.email = email;
            }
        }

        public String GetEmail()
        {
            return email;
        }

        public String GetUsername()
        {
            return username;
        }

        public String GetPassword()
        {
            return password;
        }

        public bool SetUsername(String newUsername)
        {
            if (User.IsValidUsername(newUsername))
            {
                username = newUsername;
                return true;
            }
            else return false;
        }

        public bool SetPassword(String newPassword)
        {
            if (User.IsValidPassword(newPassword))
            {
                password = newPassword;
                return true;
            }
            else return false;
        }

        public bool SetEmail(String newEmail)
        {
            if (User.IsValidEmail(newEmail))
            {
                email = newEmail;
                return true;
            }
            else return false;
        }

        public static bool IsValidEmail(String email)
        {
            if (email.Contains('@') && email.Contains('.'))
                return true;
            else return false;
        }

        public static bool IsValidPassword(String password)
        {
            Regex validChars = new Regex("^[a-zA-Z0-9.,@!-]*$");
            if (validChars.IsMatch(password) && password.Length > 7)
                return true;
            else return false;
        }

        public static bool IsValidUsername(String username)
        {
            //TODO: Check database if username is already taken
            return true;
        }
    }
}
