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
        public String _otpCreated { get; private set; }
        public bool _verified { get; private set; }

        public UserAccount(String username, String password)
        {
            _verified = false;
            _username = username;
            _password = password;
            GenerateOTP();
            //_otpCreated = DateTime.Now.Ticks.ToString();
            //var hash = SecureHasher.HashString(_otpCreated.Ticks, username);
            //_otp = hash.Substring(0, 16).Replace("-", "").Replace(" ", "");
        }
        public UserAccount(String username, String password, String otp, String otpCreated, bool verified)
        {
            _verified = verified;
            _username = username;
            _password = password;
            _otp = otp;
            _otpCreated = otpCreated;
            //var hash = SecureHasher.HashString(_otpCreated.Ticks, username);
            //_otp = hash.Substring(0, 16).Replace("-", "").Replace(" ", "");
        }

        public Response VerifyOTP(String otp)
        {
            var result = new Response();
            result.isSuccessful = false;
            var currentTime = DateTime.Now;
            if ((currentTime.Ticks - int.Parse(_otpCreated)) > 1200000000) //2 minutes in microseconds
            {
                //generate new otp
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
        public void GenerateOTP() 
        {
            var random = new Random();
            int count = 0;
            while (count < 10)
            {
                int character = random.Next(3);
                // 0-9
                if (character == 0)
                {
                    _otp = _otp + random.Next(9).ToString();
                    count++;
                }
                // a-z
                if (character == 1)
                {
                    _otp = _otp + (char)random.Next(97, 123);
                    count++;
                }
                // A-Z
                if (character == 2)
                {
                    _otp = _otp + (char)random.Next(65, 91);
                    count++;
                }
            }
            _otpCreated = DateTime.Now.Ticks.ToString();
        }
    }
}
