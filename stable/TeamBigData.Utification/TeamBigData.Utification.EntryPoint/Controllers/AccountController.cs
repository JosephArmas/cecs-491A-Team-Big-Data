using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Cryptography;
using System.Diagnostics;
using Azure;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.Models;
using System.Linq.Expressions;
using Azure.Identity;

namespace TeamBigData.Utification.EntryPoint.Controllers
{
    [BindProperties]
    public class AccountInfo
    {
        public String username { get; set; }
        public String password { get; set; }

        //TODO: add encryption Key parameter
    }

    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly SecurityManager _securityManager;
        public AccountController(SecurityManager secMan)
        {
            _securityManager = secMan;
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
            var user = new UserAccount();
            var profile = new UserProfile();
            var response = _securityManager.LoginUser(login.username, encryptedPassword, encryptor,ref user,ref profile).Result;
            if (response.isSuccessful)
            {
                tcs.SetResult(Ok(profile));
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
    }
}