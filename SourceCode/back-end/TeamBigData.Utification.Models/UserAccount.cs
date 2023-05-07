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
        public int UserID { get; private set; }
        public String Username { get; private set; }
        public String Password { get; private set; }
        public String Salt { get; private set; }
        public String Otp { get; private set; }
        public String OtpCreated { get; private set; }
        public bool Verified { get; private set; }
        public String UserHash { get; private set; }

        public UserAccount() { }
        public UserAccount(String username, String password, String salt, String userHash)
        {
            Verified = false;
            Username = username;
            Password = password;
            Salt = salt;
            UserHash = userHash;
            GenerateOTP();
        }
        public UserAccount(int userID, String username, String password, String salt, String userHash)
        {
            UserID = userID;
            Verified = false;
            Username = username;
            Password = password;
            Salt = salt;
            UserHash = userHash;
            GenerateOTP();
        }
        public UserAccount(int userID, String username, String password, String salt, String userHash, bool verified)
        {
            UserID = userID;
            Verified = verified;
            Username = username;
            Password = password;
            Salt = salt;
            UserHash = userHash;
            GenerateOTP();
        }

        public Response VerifyOTP(String otp)
        {
            var result = new Response();
            result.IsSuccessful = false;
            var currentTime = DateTime.Now;
            if ((currentTime.Ticks - float.Parse(OtpCreated)) > 1200000000) //2 minutes in microseconds
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "OTP Expired, Please Authenticate Again";
                GenerateOTP();
            }
            else
            {
                if (otp.Equals(Otp))
                {
                    Verified = true;
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
            Otp = "";
            while (count < 10)
            {
                int character = RandomNumberGenerator.GetInt32(3);
                // 0-9
                if (character == 0)
                {
                    Otp = Otp + RandomNumberGenerator.GetInt32(9);
                    count++;
                }
                // a-z
                if (character == 1)
                {
                    Otp = Otp + (char)RandomNumberGenerator.GetInt32(97, 123);
                    count++;
                }
                // A-Z
                if (character == 2)
                {
                    Otp = Otp + (char)RandomNumberGenerator.GetInt32(65, 91);
                    count++;
                }
            }
            OtpCreated = DateTime.Now.Ticks.ToString();
        }
        public string ToString()
        {
            return ",   UserID: " + UserID + ",   Username: " + Username + ", Salt: " + Salt + ",   OTP: " + Otp + ",   Verified: " + Verified + ", UserHash: " + UserHash;
        }
       
    }
}