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
        public static bool IsValidPassword(String password)
        {
            Regex passwordAllowedCharacters = new Regex(@"^[a-zA-Z0-9@.,!\s-]*$");
            if (passwordAllowedCharacters.IsMatch(password) && password.Length >= 8)
                return true;
            else
                return false;
        }

        public static bool IsValidEmail(String email)
        {
            Regex emailAllowedCharacters = new Regex(@"^[a-zA-Z0-9@.-]*$");
            if (emailAllowedCharacters.IsMatch(email) && email.Contains('@') && (!email.StartsWith("@")))
                return true;
            else
                return false;
        }

        public static bool IsValidTitle(String content)
        {
            // Find actual title from content string
            int pFrom = content.IndexOf("<h1>") + "<h1>".Length;
            int pTo = content.LastIndexOf("</h1>");

            String title = content.Substring(pFrom, pTo - pFrom);

            int countWords = title.Split().Length;

            Regex passwordAllowedCharacters = new Regex(@"^[a-zA-Z0-9áéíóúüñ¿¡ÁÉÍÓÚÜÑ@.,!\s-]*$");
            if (passwordAllowedCharacters.IsMatch(title) && title.Length <= 30 && title.Length >= 8)
                return true;
            else
                return false;
        }

        public static bool IsValidDescription(String content)
        {
            // Find actual description from content string
            int pFrom = content.IndexOf("<p>") + "<p>".Length;
            int pTo = content.LastIndexOf("</p>");

            String description = content.Substring(pFrom, pTo - pFrom);

            int countWords = description.Split().Length;

            Regex emailAllowedCharacters = new Regex(@"^[a-zA-Z0-9áéíóúüñ¿¡ÁÉÍÓÚÜÑ@.,!\s-]*$");
            if (emailAllowedCharacters.IsMatch(description) && countWords > 0 && countWords <= 150)
                return true;
            else
                return false;
        }

        public static bool AuthorizedUser(String role, String authorizedRoles)
        {
            return authorizedRoles.Contains(role);
        }
    }
}
