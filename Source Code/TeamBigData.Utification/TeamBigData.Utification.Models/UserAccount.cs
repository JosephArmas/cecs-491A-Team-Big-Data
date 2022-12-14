using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;


namespace TeamBigData.Utification.Models
{
    public class UserAccount
    {
        public String _username { get; private set; }
        public String _password { get; private set; }
        public String _otp { get; private set; }
        public DateTime _otpCreated { get; private set; }
        public bool _verified { get; private set; }

        public UserAccount(String username, String password)
        {
            _verified = false;
            _username = username;
            _password = password;
            _otpCreated = DateTime.Now;
            var hash = SecureHasher.HashString(_otpCreated.Ticks, username);
            _otp = hash.Substring(0, 16).Replace("-", "").Replace(" ", "");
        }

        public Response VerifyOTP(String otp)
        {
            var result = new Response();
            result.isSuccessful = false;
            var currentTime = DateTime.Now;
            if ((currentTime.Ticks - _otpCreated.Ticks) > 1200000000) //2 minutes in microseconds
            {
                result.isSuccessful = false;
                result.errorMessage = "OTP Expired, Please Authenticate Again";
            }
            else
            {
                if (otp.Equals(_otp))
                {
                    _verified = true;
                    result.isSuccessful = true;
                    result.errorMessage = "You have been sucessfully authenticated";
                }
                else
                {
                    result.isSuccessful = false;
                    result.errorMessage = "Error OTP does not match, Please Try Again";
                }
            }
            return result;
        }
    }
}
