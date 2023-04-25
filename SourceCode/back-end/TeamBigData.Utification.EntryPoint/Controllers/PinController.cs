using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.Models.ControllerModels;
using TeamBigData.Utification.PinManagers;

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
        
        [Route("GetAllPins")]
        [HttpGet]
        public async Task<IActionResult> GetAllPins()
        {
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
                return Unauthorized(claims.ElementAt(1).Value);
            }

                var result = await _pinManager.GetListOfAllPins(claims.ElementAt(6).Value).ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                result.isSuccessful = false;
                result.errorMessage += ", {failed: _pinManager.GetListOfAllPins}";
                return Conflict(result.errorMessage);
            }
            else
            {
                result.isSuccessful = true;
            }

            return Ok(result.data);
        }


        [Route("PostNewPin")]
        [HttpPost]
        public async Task<IActionResult> PostNewPin([FromBody]Pins newPin)
        {
            // TODO: Validate user and pin inputs

            Pin pin = new Pin(newPin._userID, newPin._lat, newPin._lng, newPin._pinType, newPin._description);

            var result = await _pinManager.SaveNewPin(pin,newPin._userhash).ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                result.errorMessage += ", {failed: _pinManager.SaveNewPin}";
                return Conflict(result.errorMessage);
            }
            else 
            { 
                return Ok(result.errorMessage); 
            }
        }

        
        [Route("CompleteUserPin")]
        [HttpPost]
        public async Task<IActionResult> CompleteUserPin([FromBody]Pins pin)
        {
            // TODO: Validate user and pin inputs

            var result = await _pinManager.MarkAsCompletedPin(pin._pinID, pin._userID, pin._userhash).ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                result.isSuccessful = false;
                result.errorMessage += ", {false: _pinManager.MarkAsCompletedPin}";
                return Conflict(result.errorMessage);
            }
            else
            {
                return Ok(result.errorMessage);
            }
        }

        [Route("ModifyPinContent")]
        [HttpPost]
        public async Task<IActionResult> ModifyPinContent([FromBody]Pins pin)
        {
            // TODO: Validate user and pin inputs

            var response = await _pinManager.ChangePinContent(pin._pinID, pin._userID, pin._description, pin._userhash).ConfigureAwait(false);
            if (!response.isSuccessful)
            {
                response.isSuccessful = false;
                response.errorMessage += ", {failed: _pinManager.ChangePinContent}";
                return Conflict(response.errorMessage);
            }
            else
            {
                return Ok(response.errorMessage);
            }
        }


        [Route("ModifyPinType")]
        [HttpPost]
        public async Task<IActionResult> ModifyPinType([FromBody]Pins pin)
        {
            // TODO: Validate user and pin inputs

            var response = await _pinManager.ChangePinType(pin._pinID, pin._userID, pin._pinType, pin._userhash);
            if (!response.isSuccessful)
            {
                response.isSuccessful = false;
                response.errorMessage += ", {failed: _pinManager.ChangePinType}";
                return Conflict(response.errorMessage);
            }
            else 
            { 
                return Ok(response.errorMessage); 
            }
        }

        [Route("DisablePin")]
        [HttpPost]
        public async Task<IActionResult> DisablePin([FromBody] Pins pin)
        {
            // TODO: Validate user and pin inputs

            var response = await _pinManager.DisablePin(pin._pinID, pin._userID, pin._userhash).ConfigureAwait(false);

            if (!response.isSuccessful)
            {
                response.isSuccessful=false;
                response.errorMessage += ", {failed: _pinManager.DisablePin}";
                return Conflict(response.errorMessage);
            }
            else
            { 
                return Ok(response.errorMessage); 
            }
        }
    }
}
 