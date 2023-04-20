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
        public int completed { get; set; }
        public string dateTime { get; set; }
    }
    [ApiController]
    [Route("[controller]")]
    public class PinController : ControllerBase
    {
        public PinController()
        {
        }

        [Authorize]
        [Route("GetAllPins")]
        [HttpGet]
        public async Task<IActionResult> GetAllPins()
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
                return Conflict("Unauthorized User.");
            }
            var pinMan = new PinManager();
            var result = await pinMan.GetListOfAllPins(claims.ElementAt(6).Value);
            if(result.isSuccessful)
            {
                return Ok(result.data);
            }
            else
            {
                return Conflict(result.errorMessage);
            }
        }
        [Authorize]
        [Route("PostNewPin")]
        [HttpPost]
        public Task<IActionResult> PostNewPin(PinInfo newPin)
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
            if (claims.ElementAt(2).Value == "Anonymouse User" || claims.ElementAt(2).Value == "Regular User")
            {
                tcs.SetResult(Conflict("Unauthorized User."));
                return tcs.Task;
            }
            Pin pin = new Pin(Int32.Parse(claims.ElementAt(0).Value), newPin.lat, newPin.lng, newPin.pinType, newPin.description, newPin.dateTime);
            var pinMan = new PinManager();
            var response = pinMan.SaveNewPin(pin, claims.ElementAt(6).Value);
            tcs.SetResult(Ok(response));
            return tcs.Task;
        }
        [Authorize]
        [Route("CompleteUserPin")]
        [HttpPost]
        public Task<IActionResult> CompleteUserPin(PinInfo newPin)
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
            if (claims.ElementAt(2).Value == "Anonymouse User")
            {
                tcs.SetResult(Conflict("Unauthorized User."));
                return tcs.Task;
            }
            var pinMan = new PinManager();
            var response = pinMan.MarkAsCompletedPin(newPin.pinID,claims.ElementAt(6).Value);
            tcs.SetResult(Ok(response));
            return tcs.Task;
        }
        [Authorize]
        [Route("ModifyPinType")]
        [HttpPost]
        public Task<IActionResult> ModifyPinType(PinInfo newPin)
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
            if (claims.ElementAt(2).Value == "Anonymouse User")
            {
                tcs.SetResult(Conflict("Unauthorized User."));
                return tcs.Task;
            }
            var pinMan = new PinManager();
            var response = pinMan.ChangePinType(newPin.pinID, newPin.pinType, claims.ElementAt(6).Value);
            tcs.SetResult(Ok(response));
            return tcs.Task;
        }
        [Authorize]
        [Route("ModifyPinContent")]
        [HttpPost]
        public Task<IActionResult> ModifyPinContent(PinInfo newPin)
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
            if (claims.ElementAt(2).Value == "Anonymouse User")
            {
                tcs.SetResult(Conflict("Unauthorized User."));
                return tcs.Task;
            }
            var pinMan = new PinManager();
            var response = pinMan.ChangePinContent(newPin.pinID, newPin.description, claims.ElementAt(6).Value);
            tcs.SetResult(Ok(response));
            return tcs.Task;
        }
        [Authorize]
        [Route("DisablePin")]
        [HttpPost]
        public Task<IActionResult> DisablePin(PinInfo newPin)
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
            if (claims.ElementAt(2).Value == "Anonymouse User")
            {
                tcs.SetResult(Conflict("Unauthorized User."));
                return tcs.Task;
            }
            var pinMan = new PinManager();
            var response = pinMan.DisablePin(newPin.pinID, claims.ElementAt(6).Value);
            tcs.SetResult(Ok(response));
            return tcs.Task;
        }
    }
}
 