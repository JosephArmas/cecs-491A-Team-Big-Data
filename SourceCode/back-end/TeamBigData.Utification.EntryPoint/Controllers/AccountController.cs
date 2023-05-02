using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.Manager;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeamBigData.Utification.Models.ControllerModels;
using ILogger = TeamBigData.Utification.Logging.Abstraction.ILogger;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Cryptography;
using System.Security.Principal;

namespace TeamBigData.Utification.EntryPoint.Controllers
{
    [ApiController]
    [Route("[controller]")] 

    public class AccountController : ControllerBase
    {
        private readonly SecurityManager _securityManager;
        private readonly IConfiguration _configuration;

        // TODO: variables to pull from jwt

        public AccountController(IConfiguration configuration,SecurityManager securityManager)
        {
            _securityManager = securityManager;
            _configuration = configuration;
        }

        [Route("authentication")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]IncomingUser login)
        {


            // Validate user role
            // Validate inputs
            if (!InputValidation.IsValidEmail(login._username) || !InputValidation.IsValidPassword(login._password))
            {
                return Conflict("Invalid credentials provided. Retry again or contact system administrator");
            }

            // Check if user is a user

            var userhash = SecureHasher.HashString(login._username, "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI");

            var dataResponse = await _securityManager.LoginUser(login._username, login._password, userhash).ConfigureAwait(false);
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

        [Route("registration")]
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody]IncomingUser newAccount)
        {

            // TODO: Decrypt incoming encryptedpassword with incoming key
            // TODO: Validate they are an anonymous user from jwt key
            //          If no JWT in Authentication header the  user is anonymous 

            var userhash = SecureHasher.HashString(newAccount._username, "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI");

            var response = await _securityManager.RegisterUser(newAccount._username,  newAccount._password, userhash).ConfigureAwait(false);
            if(response.IsSuccessful)
            {
                return Ok(response.ErrorMessage);
            }
            else
            {
                return Conflict(response.ErrorMessage + ", {failed:_securityManager.RegisterUser}");
            }
        }
    }
}