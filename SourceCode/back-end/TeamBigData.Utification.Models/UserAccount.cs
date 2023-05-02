using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
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

        public UserAccount()
        {
            _userID = 0;
            _username = "";
            _password = "";
            _salt = "";
            _otp = "";
            _verified = true;
            _userHash = "";
        }
        public UserAccount(String username, String password, String salt, String userHash)
        {
            _verified = false;
            _username = username;
            _password = password;
            _salt = salt;
            _userHash = userHash;
            GenerateOTP();
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
        }
        public UserAccount(int userID, String username, String password, String salt, String userHash, bool verified)
        {
            _userID = userID;
            _verified = verified;
            _username = username;
            _password = password;
            _salt = salt;
            _userHash = userHash;
            GenerateOTP();
        }

        public Response VerifyOTP(String otp)
        {
            var result = new Response();
            result.IsSuccessful = false;
            var currentTime = DateTime.Now;
            if ((currentTime.Ticks - float.Parse(_otpCreated)) > 1200000000) //2 minutes in microseconds
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "OTP Expired, Please Authenticate Again";
                GenerateOTP();
            }
            else
            {
                if (otp.Equals(_otp))
                {
                    _verified = true;
                    result.IsSuccessful = true;
                    result.ErrorMessage = "You have been sucessfully authenticated";
                }
                else
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "Error OTP does not match, Please Try Again";
                }
            }
            return result;
        }
        public void GenerateOTP() 
        {
            int count = 0;
            _otp = "";
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
        public string ToString()
        {
            return ",   UserID: " + _userID + ",   Username: " + _username + ", Salt: " + _salt + ",   OTP: " + _otp + ",   Verified: " + _verified + ", UserHash: " + _userHash;
        }
       
    }
}