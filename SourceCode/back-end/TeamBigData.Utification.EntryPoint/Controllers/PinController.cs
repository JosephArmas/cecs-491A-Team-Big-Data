using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.PinManagers;

namespace Utification.EntryPoint.Controllers
{
    [BindProperties]
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
    [ApiController]
    [Route("[controller]")]
    public class PinController : Controller
    {
        private readonly SecurityManager _securityManager;
        private UserAccount _userAccount;
        private UserProfile _userProfile;
        private readonly String _role;
        public PinController(SecurityManager secMan)
        {
            _securityManager = secMan;
            _userAccount = new UserAccount();
            _userProfile = new UserProfile();
        }
        [Authorize]
        [Route("health")]
        [HttpGet]
        public Task<IActionResult> HealthCheck()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            tcs.SetResult(Ok("Working"));
            return tcs.Task;
        }
        [Authorize]
        [Route("GetAllPins")]
        [HttpGet]
        public Task<IActionResult> GetAllPins()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            // get authorization header
            const string HeaderKeyName = "HeaderKey";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues headerValue);
            HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authorizationToken);
            string clean = authorizationToken;
            clean = clean.Remove(0, 7);
            // get role from JWT signature
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(clean);
            IEnumerable<Claim> claims = token.Claims;
            // check role of user
            if (claims.ElementAt(2).Value == "Anonymouse User")
            {
                tcs.SetResult(Conflict("Unauthorized User."));
                return tcs.Task;
            }
            var pinMan = new PinManager();
            List<Pin> list = pinMan.GetListOfAllPins(_userAccount);
            tcs.SetResult(Ok(list));
            return tcs.Task;
        }
        [Authorize]
        [Route("GetUserPins")]
        [HttpGet]
        public Task<IActionResult> GetUsersPins()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            //if (_userProfile.Identity.AuthenticationType == "Anonymouse User")
            //{
            //    tcs.SetResult(Conflict("Unauthorized User."));
            //    return tcs.Task;
            //}
            var pinMan = new PinManager();
            List<Pin> list = pinMan.GetListOfAllPins(_userAccount);
            tcs.SetResult(Ok(list));
            return tcs.Task;
        }
        [Authorize]
        [HttpPost]
        public Task<IActionResult> PostNewPin(PinInfo newPin)
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            if (_role == "Anonymouse User" || _role == "Regular User")
            {
                tcs.SetResult(Conflict("Unauthorized User."));
                return tcs.Task;
            }
            Pin pin = new Pin(newPin.lat, newPin.lng, newPin.description, newPin.pinType);
            var pinMan = new PinManager();
            var response = pinMan.SaveNewPin(pin, _userAccount);
            tcs.SetResult(Ok(response));
            return tcs.Task;
        }

    }
}
