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
        
        // Check if event is created within 7 days
        public static bool IsValidCreateDate(DateTime date)
        {
            TimeSpan diff = DateTime.Now - date;
            if (diff.TotalDays > 7)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        // Check if eventID is atleast over 200 -> some sort of validation that its "legitimate"
        public static bool IsValidEventID(DataResponse<int> response)
        {
            var result = Convert.ToInt32(response.Data);
            if (result >= 200 && response.IsSuccessful)
                return true;
            else
                return false;
        }
        
        // Using the bounds that Ghabe wrote in the front end
        // Checking lat and lng here again because front end can be insecure

        public static bool IsValidPinBound(double lat, double lng)
        {
            if ((lat < 42.009517 && lat > 39) && (lng < -124 || lng > -120))
            {
                return false;
            }
            else if ((lat < 39 && lat > 38) && (lng < -124 || lng > -119))
            {
                return false;
            }
            else if ((lat < 38 && lat > 37) && (lng < -123 || lng > -118))
            {
                return false;
            }
            else if ((lat < 37 && lat > 36) && (lng < -122 || lng > -117))
            {
                return false;
            }
            else if ((lat < 36 && lat > 35) && (lng < -121 || lng > -116))
            {
                return false;
            }
            else if ((lat < 35 && lat > 34) && (lng < -120 || lng > -115))
            {
                return false;
            }
            else if ((lat < 34 && lat > 33) && (lng < -119 || lng > -114))
            {
                return false;
            }
            else if ((lat < 33 && lat > 32.528832) && (lng < -118 || lng > -114))
            {
                return false;
            }
            else
            {
                return true;
            } 
            
        }
        
        
        
        
    }
}
