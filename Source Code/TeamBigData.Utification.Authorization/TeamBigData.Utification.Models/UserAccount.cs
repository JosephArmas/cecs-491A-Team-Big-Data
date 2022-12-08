using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.Models
{
    public class UserAccount
    {
        public int UserAccountID { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public DateTime OTP
        {
            get => OTP;
            set
            {
                OTP = DateTime.Now;
                //var hash = SecureHasher.HashString(OTP.Ticks, UserName);
                //OTP = hash.Substring(0, 16).Replace("-", "").Replace(" ", "");
            }
        }
        public bool Disabled { get; private set; }
        public Response VerifyOTP(String otp)
        {
            var result = new Response();
            result.isSuccessful = false;
            var currentTime = DateTime.Now;
            if ((currentTime.Ticks - OTP.Ticks) > 1200000000) //2 minutes in microseconds
            {
                result.isSuccessful = false;
                result.errorMessage = "OTP Expired, Please Authenticate Again";
            }
            else
            {
                if (otp.Equals(OTP))
                {

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