using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.SQLDataAccess;
using System.Text.RegularExpressions;

namespace TeamBigData.Utification.Registration
{
    public class RegistrationServices
    {
        public bool IsValidUsername(String username)
        {
            Regex usernameAllowedCharacters = new Regex(@"^[a-zA-Z0-9@.-]*$");
            if (usernameAllowedCharacters.IsMatch(username) && username.Length >= 8)
                return true;
            else
                return false;
        }
        
        public bool IsValidPassword(String password)
        {
            Regex passwordAllowedCharacters = new Regex(@"^[a-zA-Z0-9@.,!-]*$");
            if (passwordAllowedCharacters.IsMatch(password) && password.Length >= 8)
                return true;
            else
                return false;
        }

        public bool IsValidEmail(String email)
        {
            Regex emailAllowedCharacters = new Regex(@"^[a-zA-Z0-9@.-]*$");
            if (emailAllowedCharacters.IsMatch(email) && email.Contains('@'))
                return true;
            else
                return false;
        }

        public User GetNewUserInfo()
        {
            Console.WriteLine("Please enter your perferred username");
            String username = Console.ReadLine();
            while(!IsValidUsername(username))
            {
                if(username.Length < 8)
                {
                    Console.WriteLine("You have invalid characters in your username");
                    Console.WriteLine("Please enter a new perferred username with only letters, numbers, . - or @");
                }
                else
                {
                    Console.WriteLine("Your perferred username is too short");
                    Console.WriteLine("Please enter a new username at least 8 characters long");
                }
                username = Console.ReadLine();
            }
            Console.WriteLine("Please enter your email");
            String email = Console.ReadLine();
            while(!IsValidEmail(email))
            {
                if(!email.Contains('@'))
                {
                    Console.WriteLine("The email you have entered is not a valid email");
                    Console.WriteLine("Pleas enter a new valid email");
                }
                else
                {
                    Console.WriteLine("You have invalid characters in your email");
                    Console.WriteLine("Please enter a new email with only letters, numbers, . - or @");
                }
                email = Console.ReadLine();
            }
            Console.WriteLine("Please enter your passphrase to login");
            String password = Console.ReadLine();
            while (!IsValidPassword(password))
            {
                if(password.Length >= 8)
                {
                    Console.WriteLine("You have invalid characters in your passphrase");
                    Console.WriteLine("Please enter a new passphrase with only letters, numbers, . , - @ or !");
                }
                else
                {
                    Console.WriteLine("Your passphrase is too short");
                    Console.WriteLine("Please enter a new passphrase at least 8 characters long");
                }
                password = Console.ReadLine();
            }
            Random rng = new Random();
            int userNumber = rng.Next(0, 9999);
            if(userNumber < 1000 && userNumber > 100)
            {
                username = username + "#0" + userNumber;
            }
            if(userNumber < 100 && userNumber > 10)
            {
                username = username + "#00" + userNumber;
            }
            if(userNumber < 10)
            {
                username = username + "#000" + userNumber;
            }
            User newUser = new User(username, password, email);
            return newUser;
        }
    }
}
