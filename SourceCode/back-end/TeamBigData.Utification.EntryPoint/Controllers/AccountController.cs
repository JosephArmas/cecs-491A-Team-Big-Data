using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.PinManagers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

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
        private UserAccount _userAccount;
        private UserProfile _userProfile;
        private readonly IConfiguration _configuration;
        /*public AccountController(SecurityManager secMan)
        {
            _securityManager = secMan;
            _userAccount = new UserAccount();
            _userProfile = new UserProfile();
        }*/

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;

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
        
        
        // Allow anonymous doesn't check for authorization header
        [AllowAnonymous]
        [Route("authentication")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] AccountInfo login)
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            var encryptor = new Encryptor();
            var encryptedPassword = encryptor.encryptString(login.password);
            _userAccount = new UserAccount();
            _userProfile = new UserProfile();
            var secMan = new SecurityManager();
            var response = await secMan.LoginUser(login.username, encryptedPassword, encryptor, _userProfile);
            if (response.isSuccessful)
            {
                //Create JWT token with our claims
                var tokenHandler = new JwtSecurityTokenHandler();

                var keyDetail = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

                //Token specifications
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, _userAccount._userID.ToString()),
                    new Claim(ClaimTypes.Name, _userAccount._username),
                    new Claim(ClaimTypes.Role, _userProfile.Identity.AuthenticationType),
                    new Claim("authenticated", "true", ClaimValueTypes.String),
                    new Claim("otp", _userAccount._otp, ClaimValueTypes.String),
                    new Claim("otpCreated", _userAccount._otpCreated, ClaimValueTypes.String),
                    new Claim("userHash", _userAccount._userHash, ClaimValueTypes.String)
                };

                
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Audience = _configuration["JWT:Audience"],
                    Issuer = _configuration["JWT:Issuer"],
                    Expires = DateTime.UtcNow.AddHours(2),
                    Subject = new ClaimsIdentity(claims),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyDetail), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                
                //Send token signature back to client
                return Ok(tokenHandler.WriteToken(token));
            }
            else
            {
                return Conflict(response.errorMessage);
            }
        }

        //Authorize requires JWT signature in authorization header
        [Authorize]
        [Route("authentication")]
        [HttpGet]
        public Task<IActionResult> Logout()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            var user = new UserProfile();
            tcs.SetResult(Ok(user));
            return tcs.Task;
        }

        [AllowAnonymous]
        [Route("registration")]
        [HttpPost]
        public Task<IActionResult> CreateAccount(AccountInfo newAccount)
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            var encryptor = new Encryptor();
            var secMan = new SecurityManager();
            var encryptedPassword = encryptor.encryptString(newAccount.password);
            var response = secMan.RegisterUser(newAccount.username,  encryptedPassword, encryptor).Result;
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