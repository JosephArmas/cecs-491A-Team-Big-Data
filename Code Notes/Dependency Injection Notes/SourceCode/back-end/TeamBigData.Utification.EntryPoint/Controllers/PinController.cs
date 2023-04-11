using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.PinManagers;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.PinServices;
using TeamBigData.Utification.Models.DTO;

namespace Utification.EntryPoint.Controllers
{  
    [ApiController]
    [Route("[controller]")]
    public class PinController : ControllerBase
    {
        private readonly PinManager _pinManager;
        public PinController(PinManager pinManager)
        {
            _pinManager = pinManager;
        }


#if DEBUG
        [Route("health")]
        [HttpGet]
        public Task<IActionResult> HealthCheck()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            tcs.SetResult(Ok("Working"));
            return tcs.Task;
        }
#endif

        /*[Route("GetAllPins")]
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
                tcs.SetResult(Conflict("Unauthorized User."));
                return tcs;
            }
            var pinMan = new PinManager();
            List<Pin> list = pinMan.GetListOfAllPins(claims.ElementAt(6).Value);
            tcs.SetResult(Ok(list));
            return tcs;
        }*/

        [Route("GetAllPins")]
        [HttpGet]
        public async Task<IActionResult> GetAllPins()
        {

            var pins = await _pinManager.GetListOfAllPins("test").ConfigureAwait(false);
            return Ok(pins);
        }

        [Route("PostNewPin")]
        [HttpPost]
        public async Task<IActionResult> PostNewPin([FromBody] CreatePinDto createPin)
        {
            //var pin = new Pin(createPin._userID,createPin._lat,createPin._lng,createPin._pinType,createPin._description);
            var result = await _pinManager.SaveNewPin(createPin, "test").ConfigureAwait(false);
            return Ok(result);
        }

        /*[Authorize]
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
        }*/
    }
}
 