using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.SQLDataAccess.DTO
{
    public class AuthenticateUserResponse
    {
        public int _userId { get; set; }
        public String _username { get; set; }
        public String _otp { get; set; }
        public String _otpCreated { get; set; }
        public String _role { get; set; }
        public String _userhash { get; set; }
        public AuthenticateUserResponse(int userId, string username, string otp, string otpCreated, string role, string userhash)
        {
            _userId = userId;
            _username = username;
            _otp = otp;
            _otpCreated = otpCreated;
            _role = role;
            _userhash = userhash;
        }
    }
}