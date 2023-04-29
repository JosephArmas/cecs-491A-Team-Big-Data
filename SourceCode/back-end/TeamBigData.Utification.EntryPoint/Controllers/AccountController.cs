using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.Manager;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeamBigData.Utification.Models;
using ILogger = TeamBigData.Utification.Logging.Abstraction.ILogger;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Cryptography;
using System.Security.Principal;

namespace TeamBigData.Utification.EntryPoint.Controllers
{
    [BindProperties]


    [ApiController]
    [Route("[controller]")]

    public class AccountController : ControllerBase
    {
        // TODO: Need to take out of Controller and put into different file
        // Taking it out will break current code

        [BindProperties]
        public class IncomingUser
        {
            public String Username { get; set; }
            public String Password { get; set; }
        }

        private readonly SecurityManager _securityManager;
        private readonly IConfiguration _configuration;
        private readonly InputValidation _inputValidation;

        // TODO: variables to pull from jwt

        public AccountController(IConfiguration configuration,SecurityManager securityManager, InputValidation inputValidation)
        {
            _securityManager = securityManager;
            _configuration = configuration;
            _inputValidation = inputValidation;
        }

        [Route("authentication")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]IncomingUser login)
        {


            // Validate user role
            // Validate inputs
            if (!await _inputValidation.IsValidEmail(login.Username).ConfigureAwait(false) || !await _inputValidation.IsValidPassword(login.Password).ConfigureAwait(false))
            {
                return Conflict("Invalid credentials provided. Retry again or contact system administrator");
            }

            // Check if user is a user

            var userhash = SecureHasher.HashString(login.Username, "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI");

            var dataResponse = await _securityManager.LoginUser(login.Username, login.Password, userhash).ConfigureAwait(false);
            if (!dataResponse.isSuccessful)
            {
                return Conflict(dataResponse.errorMessage + ", {failed: _securityManager.LoginUser}");
            }

            // Create JWT
            var tokenHandler = new JwtSecurityTokenHandler();

            var keyDetail = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

            //Token specifications
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, dataResponse.data._userId.ToString()),
                new Claim(ClaimTypes.Name, dataResponse.data._username),
                new Claim(ClaimTypes.Role, dataResponse.data._role),
                new Claim("authenticated", "true", ClaimValueTypes.String),
                new Claim("otp", dataResponse.data._otp, ClaimValueTypes.String),
                new Claim("otpCreated", dataResponse.data._otpCreated, ClaimValueTypes.String),
                new Claim("userhash", dataResponse.data._userhash, ClaimValueTypes.String)
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

        
        [Route("authentication")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            return Ok("");
        }

        [Route("delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteAccount([FromBody] IncomingUser login)
        {
            // TODO: Validate if is  this user or an admin

            // Do action
            var response = await _securityManager.DeleteUser(login.Username).ConfigureAwait(false);
            if (!response.isSuccessful)
            {
                return Conflict(response.errorMessage + ", {failed: _securityManager.DeleteUser}");
            }

            return Ok(response.errorMessage);
        }

        [Route("registration")]
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody]IncomingUser newAccount)
        {

            // TODO: Decrypt incoming encryptedpassword with incoming key
            // TODO: Validate they are an anonymous user from jwt key
            //          If no JWT in Authentication header the  user is anonymous 

            var userhash = SecureHasher.HashString("5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI", newAccount.Username);

            var response = await _securityManager.RegisterUser(newAccount.Username,  newAccount.Password, userhash).ConfigureAwait(false);
            if(response.isSuccessful)
            {
                return Ok(response.errorMessage);
            }
            else
            {
                return Conflict(response.errorMessage + ", {failed:_securityManager.RegisterUser}");
            }
        }
    }
}