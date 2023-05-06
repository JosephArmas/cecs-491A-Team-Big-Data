using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
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
<<<<<<< HEAD
        private readonly String _role;
        private readonly String _userhash;
        private readonly int _userId;
        private readonly IConfiguration _configuration;
        public PinController(PinManager pinManager, IConfiguration configuration)
        {
            _pinManager = pinManager;

            if (Request == null)
            {
                _role = "Anonymous User";
                _userhash = "";
                _userId = 0;

            }
            else
            {
                const string HeaderKeyName = "HeaderKey";
                Request.Headers.TryGetValue(HeaderKeyName, out StringValues headerValue);
                HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authorizationToken);

                // Get role from JWT signature.
                string clean = authorizationToken;
                clean = clean.Remove(0, 7);

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(clean);
                IEnumerable<Claim> claims = token.Claims;

                // Get whats needed from JWT.
                _role = claims.ElementAt(2).Value;
                _userhash = claims.ElementAt(6).Value;
                _userId = Convert.ToInt32(claims.ElementAt(0).Value);
            }

            _configuration = configuration;
=======
        public PinController(PinManager pinManager)
        {
            _pinManager = pinManager;
>>>>>>> parent of 7553d278 (Trying to integrate features together and fixing any merging problems)
        }

        [Route("GetAllPins")]
        [HttpGet]
        public async Task<IActionResult> GetAllPins()
        {
<<<<<<< HEAD
            if (InputValidation.AuthorizedUser(_role, _configuration["PinAuthorization:GetAllPins"]))
=======
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
>>>>>>> parent of 7553d278 (Trying to integrate features together and fixing any merging problems)
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
<<<<<<< HEAD
            if (InputValidation.AuthorizedUser(_role, _configuration["PinAuthorization:PostNewPin"]))
            {
                return Unauthorized("Unsupported User.");
            }

            if (!InputValidation.IsValidTitle(newPin.Description) || !InputValidation.IsValidDescription(newPin.Description))
            {
                return Conflict("Invalid Pin Content.");
            }

            Pin pin = new Pin(newPin.UserID, newPin.Lat, newPin.Lng, newPin.PinType, newPin.Description);

            var result = await _pinManager.SaveNewPin(pin,newPin.Userhash).ConfigureAwait(false);
=======
            // TODO: Validate user and pin inputs

            Pin pin = new Pin(newPin._userID, newPin._lat, newPin._lng, newPin._pinType, newPin._description);
>>>>>>> parent of 7553d278 (Trying to integrate features together and fixing any merging problems)

            var result = await _pinManager.SaveNewPin(pin,newPin._userhash).ConfigureAwait(false);
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
<<<<<<< HEAD
            if (InputValidation.AuthorizedUser(_role, _configuration["PinAuthorization:CompleteUserPin"]))
            {
                return Unauthorized("Unsupported User.");
            }

            var result = await _pinManager.DeleteUserPin(pin.PinID, pin.Userhash).ConfigureAwait(false);
=======
            // TODO: Validate user and pin inputs
>>>>>>> parent of 7553d278 (Trying to integrate features together and fixing any merging problems)

            var result = await _pinManager.MarkAsCompletedPin(pin._pinID, pin._userID, pin._userhash).ConfigureAwait(false);
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
<<<<<<< HEAD
            if (InputValidation.AuthorizedUser(_role, _configuration["PinAuthorization:ModifyPinContent"]) || _userId != pin.UserID)
            {
                return Unauthorized("Unsupported User.");
            }

            if (!InputValidation.IsValidTitle(pin.Description) || !InputValidation.IsValidDescription(pin.Description) || !(pin.PinType >= 0 || pin.PinType <= 5))
            {
                return Conflict("Invalid Pin Description.");
            }

            var response = await _pinManager.ChangePinContent(pin.PinID, pin.UserID, pin.Description, pin.Userhash).ConfigureAwait(false);
=======
            // TODO: Validate user and pin inputs
>>>>>>> parent of 7553d278 (Trying to integrate features together and fixing any merging problems)

            var response = await _pinManager.ChangePinContent(pin._pinID, pin._userID, pin._description, pin._userhash).ConfigureAwait(false);
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
<<<<<<< HEAD
            if (InputValidation.AuthorizedUser(_role, _configuration["PinAuthorization:ModifyPinType"]) || _userId != pin.UserID)
            {
                return Unauthorized("Unsupported User.");
            }

            if (!InputValidation.IsValidTitle(pin.Description) || !InputValidation.IsValidDescription(pin.Description) || !(pin.PinType >= 0 || pin.PinType <= 5))
            {
                return Conflict("Invalid Pin.");
            }

            var response = await _pinManager.ChangePinType(pin.PinID, pin.UserID, pin.PinType, pin.Userhash);
=======
            // TODO: Validate user and pin inputs
>>>>>>> parent of 7553d278 (Trying to integrate features together and fixing any merging problems)

            var response = await _pinManager.ChangePinType(pin._pinID, pin._userID, pin._pinType, pin._userhash);
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
<<<<<<< HEAD
            if (InputValidation.AuthorizedUser(_role, _configuration["PinAuthorization:DisablePin"]))
            {
                return Unauthorized("Unsupported User.");
            }
=======
            // TODO: Validate user and pin inputs
>>>>>>> parent of 7553d278 (Trying to integrate features together and fixing any merging problems)

            var response = await _pinManager.DisablePin(pin._pinID, pin._userID, pin._userhash).ConfigureAwait(false);

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

        [Route("LoadMap")]
        [HttpGet]
        public async Task<IActionResult> GetMapApiKey()
        {
            if (!_configuration["PinAuthorization:GetMapApiKey"].Contains(_role))
            {
                return Unauthorized("Unsupported User.");
            }

            return Ok(_configuration["GoogleMapsApiKey"]);
        }
    }
}
 