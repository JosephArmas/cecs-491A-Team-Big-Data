using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Cryptography;
using System.Diagnostics;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.PinManagers;
using System.Linq.Expressions;
using Azure.Identity;
using System.Net.NetworkInformation;
using System.Security.Principal;
using TeamBigData.Utification.SQLDataAccess.Abstractions;

namespace TeamBigData.Utification.EntryPoint.Controllers
{
    [BindProperties]
    public class AccountInfo
    {
        public String username { get; set; }
        public String password { get; set; }

        //TODO: add encryption Key parameter
    }
    
    public class PinInfo
    {
        public int pinID { get; set; }
        public int userID { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public int pinType { get; set; }
        public string description { get; set; }
        public int disabled { get; set; }
    }

    public class UpdateUserProfileInfo
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address { get; set; }
        public DateTime birthday { get; set; }

    }
    
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly SecurityManager _securityManager;
        private UserAccount _userAccount;
        private UserProfile _userProfile;
        public AccountController(SecurityManager secMan)
        {
            _securityManager = secMan;
            _userAccount = new UserAccount();
            _userProfile = new UserProfile();
        }

        [Route("health")]
        [HttpGet]
        public Task<IActionResult> HealthCheck()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            tcs.SetResult(Ok("Working"));
            return tcs.Task;
        }
        //try curl -i -X GET "https://localhost:7259/account/health"

        //2 Main ways to send parameters forms which curl uses on the top
        //curl -d "username=testUser@yahoo.com&password=password" -X POST "https://localhost:7259/account/authentication"
        //And in the body of the http request
        //curl -H "Content-Type: application/json" -d "{"username":"testUser@yahoo.com", "password":"password"}" -X POST "https://localhost:7259/account/authentication"
        //You have to specify where its coming from with the tags on the parameters, either make it [FromForm] or [FromBody] 

        //curl -H "Content-Type: application/json" -d "@data.json" -X Post "https://localhost:7259/account/authentication"
        [Route("authentication")]
        [HttpPost]
        public Task<IActionResult> Login([FromBody] AccountInfo login)
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString(login.password);
            _userAccount = new UserAccount();
            _userProfile = new UserProfile();
            var response = _securityManager.LoginUser(login.username, encryptedPassword, encryptor,ref _userAccount,ref _userProfile).Result;
            if (response.isSuccessful)
            {
                tcs.SetResult(Ok(_userProfile));
            }
            else
            {
                tcs.SetResult(Conflict(response.errorMessage));
            }
            return tcs.Task;
        }

        [Route("authentication")]
        [HttpGet]
        public Task<IActionResult> Logout()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            var user = new UserProfile();
            tcs.SetResult(Ok(user));
            return tcs.Task;
        }

        [Route("registration")]
        [HttpPost]
        public Task<IActionResult> CreateAccount(AccountInfo newAccount)
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString(newAccount.password);
            var response = _securityManager.RegisterUser(newAccount.username, encryptedPassword, encryptor).Result;
            if(response.isSuccessful)
            {
                tcs.SetResult(Ok(response.errorMessage));
            }
            else
            {
                tcs.SetResult(Conflict(response.errorMessage));
            }
            return tcs.Task;
        }

        [Route("pin")]
        [HttpGet]
        public Task<IActionResult> GetAllPins()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            if (_userProfile.Identity.AuthenticationType == "Anonymouse User")
            {
                tcs.SetResult(Conflict("Unauthorized User."));
                return tcs.Task;
            }
            var pinMan = new PinManager();
            List<Pin> list = pinMan.GetListOfAllPins(_userAccount);
            tcs.SetResult(Ok(list));
            return tcs.Task;
        }
        [Route("pin")]
        [HttpPost]
        public Task<IActionResult> PostNewPin(PinInfo newPin)
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            if (_userProfile.Identity.AuthenticationType == "Anonymouse User")
            {
                tcs.SetResult(Conflict("Unauthorized User."));
                return tcs.Task;
            }
            Pin pin = new Pin(newPin.lat,newPin.lng,newPin.description,newPin.pinType);
            var pinMan = new PinManager();
            var response = pinMan.SaveNewPin(pin,_userAccount);
            tcs.SetResult(Ok(response));
            return tcs.Task;
        }

        [Route("updateProfile")]
        [HttpPost]
        public Task<IActionResult> UpdateProfile(UpdateUserProfileInfo updateProfile)
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            _userProfile = new UserProfile();
            var response = _securityManager.UpdateProfile(_userProfile._userID,updateProfile.firstName, updateProfile.lastName, ref _userProfile, updateProfile.birthday, updateProfile.address).Result;
            if (response.isSuccessful)
            {
                tcs.SetResult(Ok(response.errorMessage));
            }
            else
            {
                tcs.SetResult(Conflict(response.errorMessage));
            }

            return tcs.Task;
        }
    }
}