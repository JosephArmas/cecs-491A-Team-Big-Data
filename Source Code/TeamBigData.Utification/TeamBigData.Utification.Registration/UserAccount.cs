using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Security;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.AccountServices
{
    public class UserAccount
    {
        private String _username;
        private String _otp;
        private DateTime _otpCreated;
        private bool _verified;

        public UserAccount(String username)
        {
            _verified = false;
            _username = username;
            _otpCreated = DateTime.Now;
            var hash = SecureHasher.HashString(_otpCreated.Ticks, username);
            _otp = hash.Substring(0, 16).Replace("-", "").Replace(" ", "");
        }

        public String GetUsername()
        {
            return _username;
        }

        public String GetOTP()
        {
            return _otp;
        }

        public DateTime GetOTPCreated()
        {
            return _otpCreated;
        }

        public bool IsVerified() 
        { 
            return _verified;
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
