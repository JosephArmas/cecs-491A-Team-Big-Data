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
        private readonly String _role;
        private readonly String _userhash;
        private readonly int _userId;
        public PinController(PinManager pinManager)
        {
            _pinManager = pinManager;

            // Get authorization header.
            const string HeaderKeyName = "HeaderKey";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues headerValue);
            HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authorizationToken);
            string clean = authorizationToken;
            clean = clean.Remove(0, 7);

            // Get role from JWT signature.
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(clean);
            IEnumerable<Claim> claims = token.Claims;

            // Get whats needed from JWT.
            _role = claims.ElementAt(2).Value;
            _userhash = claims.ElementAt(6).Value;
            _userId = Convert.ToInt32(claims.ElementAt(0).Value);
        }
        
        [Route("GetAllPins")]
        [HttpGet]
        public async Task<IActionResult> GetAllPins()
        {
            if (_role != "Regular User" || _role != "Reputable User" || _role != "Service User" || _role != "Admin User")
            {
                return Unauthorized("Unsupported User.");
            }

            var result = await _pinManager.GetListOfAllEnabledPins(_userhash).ConfigureAwait(false);

            if (!result.IsSuccessful)
            {
                result.IsSuccessful = false;
                result.ErrorMessage += ", {failed: _pinManager.GetListOfAllPins}";
                return Conflict(result.ErrorMessage);
            }
            else
            {
                result.IsSuccessful = true;
            }

            return Ok(result.Data);
        }


        [Route("PostNewPin")]
        [HttpPost]
        public async Task<IActionResult> PostNewPin([FromBody]Pins newPin)
        {
            if (_role != "Reputable User" || _role != "Service User" || _role != "Admin User")
            {
                return Unauthorized("Unsupported User.");
            }

            if (!InputValidation.IsValidTitle(newPin.Description) || !InputValidation.IsValidDescription(newPin.Description))
            {
                return Conflict("Invalid Pin Description.");
            }

            Pin pin = new Pin(newPin.UserID, newPin.Lat, newPin.Lng, newPin.PinType, newPin.Description);

            var result = await _pinManager.SaveNewPin(pin,newPin.Userhash).ConfigureAwait(false);

            if (!result.IsSuccessful)
            {
                result.ErrorMessage += ", {failed: _pinManager.SaveNewPin}";
                return Conflict(result.ErrorMessage);
            }
            else 
            { 
                return Ok(result.ErrorMessage); 
            }
        }

        
        [Route("CompleteUserPin")]
        [HttpPost]
        public async Task<IActionResult> CompleteUserPin([FromBody]Pins pin)
        {
            if (_role != "Regular User" || _role != "Reputable User" || _role != "Service User" || _role != "Admin User")
            {
                return Unauthorized("Unsupported User.");
            }

            var result = await _pinManager.DeleteUserPin(pin.PinID, pin.Userhash).ConfigureAwait(false);

            if (!result.IsSuccessful)
            {
                result.IsSuccessful = false;
                result.ErrorMessage += ", {false: _pinManager.MarkAsCompletedPin}";
                return Conflict(result.ErrorMessage);
            }
            else
            {
                return Ok(result.ErrorMessage);
            }
        }

        [Route("ModifyPinContent")]
        [HttpPost]
        public async Task<IActionResult> ModifyPinContent([FromBody]Pins pin)
        {
            if (_role != "Admin User" || _userId != pin.UserID)
            {
                return Unauthorized("Unsupported User.");
            }

            if (!InputValidation.IsValidTitle(pin.Description) || !InputValidation.IsValidDescription(pin.Description) || !(pin.PinType >= 0 || pin.PinType <= 5))
            {
                return Conflict("Invalid Pin Description.");
            }

            var response = await _pinManager.ChangePinContent(pin.PinID, pin.UserID, pin.Description, pin.Userhash).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.ErrorMessage += ", {failed: _pinManager.ChangePinContent}";
                return Conflict(response.ErrorMessage);
            }
            else
            {
                return Ok(response.ErrorMessage);
            }
        }


        [Route("ModifyPinType")]
        [HttpPost]
        public async Task<IActionResult> ModifyPinType([FromBody]Pins pin)
        {
            if (_role != "Admin User" || _userId != pin.UserID)
            {
                return Unauthorized("Unsupported User.");
            }

            if (!InputValidation.IsValidTitle(pin.Description) || !InputValidation.IsValidDescription(pin.Description) || !(pin.PinType >= 0 || pin.PinType <= 5))
            {
                return Conflict("Invalid Pin Description.");
            }

            var response = await _pinManager.ChangePinType(pin.PinID, pin.UserID, pin.PinType, pin.Userhash);

            if (!response.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.ErrorMessage += ", {failed: _pinManager.ChangePinType}";
                return Conflict(response.ErrorMessage);
            }
            else 
            { 
                return Ok(response.ErrorMessage); 
            }
        }

        [Route("DisablePin")]
        [HttpPost]
        public async Task<IActionResult> DisablePin([FromBody] Pins pin)
        {
            if (_role != "Admin User")
            {
                return Unauthorized("Unsupported User.");
            }

            var response = await _pinManager.DisablePin(pin.PinID, pin.UserID, pin.Userhash).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                response.IsSuccessful =false;
                response.ErrorMessage += ", {failed: _pinManager.DisablePin}";
                return Conflict(response.ErrorMessage);
            }
            else
            { 
                return Ok(response.ErrorMessage); 
            }
        }
    }
}
 