using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TeamBigData.Utification.ErrorResponse
{
    public class InputValidation
    {
        public async Task<bool> IsValidPassword(String password)
        {
            Regex passwordAllowedCharacters = new Regex(@"^[a-zA-Z0-9@.,!\s-]*$");
            if (passwordAllowedCharacters.IsMatch(password) && password.Length >= 8)
                return true;
            else
                return false;
        }

        public async Task<bool> IsValidEmail(String email)
        {
            Regex emailAllowedCharacters = new Regex(@"^[a-zA-Z0-9@.-]*$");
            if (emailAllowedCharacters.IsMatch(email) && email.Contains('@') && (!email.StartsWith("@")))
                return true;
            else
                return false;
        }
    }
}
