using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;



namespace TeamBigData.Utification.Models
{
    public class UserAccount
    {
        public int _userID { get; private set; }
        public String _username { get; private set; }
        public String _password { get; private set; }
        public String _salt { get; private set; }
        public String _otp { get; private set; }
        public String _otpCreated { get; private set; }
        public bool _verified { get; private set; }
        public String _userHash { get; private set; }

        public UserAccount(String username, String password, String salt, String userHash)
        {
            _verified = false;
            _username = username;
            _password = password;
            _salt = salt;
            _userHash = userHash;
            GenerateOTP();
            //_otpCreated = DateTime.Now.Ticks.ToString();
            //var hash = SecureHasher.HashString(_otpCreated.Ticks, username);
            //_otp = hash.Substring(0, 16).Replace("-", "").Replace(" ", "");
        }
        public UserAccount(int userID, String username, String password, String salt, String userHash)
        {
            _userID = userID;
            _verified = false;
            _username = username;
            _password = password;
            _salt = salt;
            _userHash = userHash;
            GenerateOTP();
            //_otpCreated = DateTime.Now.Ticks.ToString();
            //var hash = SecureHasher.HashString(_otpCreated.Ticks, username);
            //_otp = hash.Substring(0, 16).Replace("-", "").Replace(" ", "");
        }
        public UserAccount(int userID, String username, String password, String salt, String userHash, bool verified)
        {
            _userID = userID;
            _verified = verified;
            _username = username;
            _password = password;
            GenerateOTP();
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
            int count = 0;
            while (count < 10)
            {
                int character = RandomNumberGenerator.GetInt32(3);
                // 0-9
                if (character == 0)
                {
                    _otp = _otp + RandomNumberGenerator.GetInt32(9);
                    count++;
                }
                // a-z
                if (character == 1)
                {
                    _otp = _otp + (char)RandomNumberGenerator.GetInt32(97, 123);
                    count++;
                }
                // A-Z
                if (character == 2)
                {
                    _otp = _otp + (char)RandomNumberGenerator.GetInt32(65, 91);
                    count++;
                }
            }
            _otpCreated = DateTime.Now.Ticks.ToString();
        }
    }
}
