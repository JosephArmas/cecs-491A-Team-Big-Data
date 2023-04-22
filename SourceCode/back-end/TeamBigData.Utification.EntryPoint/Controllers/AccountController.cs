using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
            public String _username { get; set; }
            public String _password { get; set; }
        }

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
            // Check if user is a user
            var dataResponse = await _securityManager.LoginUser(login._username, login._password).ConfigureAwait(false);
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
                new Claim("userHash", dataResponse.data._userhash, ClaimValueTypes.String)
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


            var response = await _securityManager.RegisterUser(newAccount._username,  newAccount._password).ConfigureAwait(false);
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