using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.SQLDataAccess.DTO
{
    public class AuthenticateUserResponse
    {
        public int UserId { get; set; }
        public String Username { get; set; }
        public String Otp { get; set; }
        public String OtpCreated { get; set; }
        public String Role { get; set; }
        public String Userhash { get; set; }
        public AuthenticateUserResponse(int userId, string username, string otp, string otpCreated, string role, string userhash)
        {
            UserId = userId;
            Username = username;
            Otp = otp;
            OtpCreated = otpCreated;
            Role = role;
            Userhash = userhash;
        }
    }
}
