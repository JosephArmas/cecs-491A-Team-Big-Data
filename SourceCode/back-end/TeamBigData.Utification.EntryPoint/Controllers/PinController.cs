using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
using TeamBigData.Utification.Logging.Abstraction;
using ILogger = TeamBigData.Utification.Logging.Abstraction.ILogger;

namespace Utification.EntryPoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PinController : ControllerBase
    {
        private readonly PinManager _pinManager;
        private String _role;
        private String _userhash;
        private int _userId;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public PinController(PinManager pinManager, IConfiguration configuration, ILogger logger)
        {
            _pinManager = pinManager;
            _configuration = configuration;
            _role = "Anonymous User";
            _userhash = "";
            _userId = 0;
            _logger = logger;
        }

        [Route("GetAllPins")]
        [HttpGet]
        public async Task<IActionResult> GetAllPins()
        {
            try
            {
                await LoadUser().ConfigureAwait(false);
                if (!InputValidation.AuthorizedUser(_role, _configuration["PinAuthorization:GetAllPins"]))
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
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }


        [Route("PostNewPin")]
        [HttpPost]
        public async Task<IActionResult> PostNewPin([FromBody] Pins newPin)
        {
            try
            {
                await LoadUser().ConfigureAwait(false);
                if (!InputValidation.AuthorizedUser(_role, _configuration["PinAuthorization:PostNewPin"]))
                {
                    return Unauthorized("Unsupported User.");
                }

                if (!InputValidation.IsValidTitle(newPin.Description) || !InputValidation.IsValidDescription(newPin.Description))
                {
                    return Conflict("Invalid Pin Content.");
                }

                Pin pin = new Pin(newPin.UserID, newPin.Lat, newPin.Lng, newPin.PinType, newPin.Description);

                var result = await _pinManager.SaveNewPin(pin, newPin.Userhash).ConfigureAwait(false);

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
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }


        [Route("CompleteUserPin")]
        [HttpPost]
        public async Task<IActionResult> CompleteUserPin([FromBody] Pins pin)
        {
            try
            {
                await LoadUser().ConfigureAwait(false);
                if (!InputValidation.AuthorizedUser(_role, _configuration["PinAuthorization:CompleteUserPin"]))
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
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [Route("ModifyPinContent")]
        [HttpPost]
        public async Task<IActionResult> ModifyPinContent([FromBody] Pins pin)
        {
            try
            {   
                await LoadUser().ConfigureAwait(false);
                if (!InputValidation.AuthorizedUser(_role, _configuration["PinAuthorization:ModifyPinContent"]) || (_userId != pin.UserID && _role != "Admin User"))
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
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }                                                       


        [Route("ModifyPinType")]
        [HttpPost]
        public async Task<IActionResult> ModifyPinType([FromBody] Pins pin)
        {
            try
            {
                await LoadUser().ConfigureAwait(false);
                if (!InputValidation.AuthorizedUser(_role, _configuration["PinAuthorization:ModifyPinType"]) || (_userId != pin.UserID && _role != "Admin User"))
                {
                    return Unauthorized("Unsupported User.");
                }

                if (!InputValidation.IsValidTitle(pin.Description) || !InputValidation.IsValidDescription(pin.Description) || !(pin.PinType >= 0 || pin.PinType <= 5))
                {
                    return Conflict("Invalid Pin.");
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
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [Route("DisablePin")]
        [HttpPost]
        public async Task<IActionResult> DisablePin([FromBody] Pins pin)
        {
            try
            {
                await LoadUser().ConfigureAwait(false);
                if (!InputValidation.AuthorizedUser(_role, _configuration["PinAuthorization:DisablePin"]))
                {
                    return Unauthorized("Unsupported User.");
                }

                var response = await _pinManager.DisablePin(pin.PinID, pin.UserID, pin.Userhash).ConfigureAwait(false);

                if (!response.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.ErrorMessage += ", {failed: _pinManager.DisablePin}";
                    return Conflict(response.ErrorMessage);
                }
                else
                {
                    return Ok(response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }

        }

        [Route("LoadMap")]
        [HttpGet]
        public async Task<IActionResult> GetMapApiKey()
        {
            try
            {
                await LoadUser().ConfigureAwait(false);
                if (!InputValidation.AuthorizedUser(_role, _configuration["MapAuthorization:LoadMap"]))
                {
                    await _logger.Logs(new Log(0, "Error", _userhash, "GetMapApiKey", "Data", "Unsupported User is trying to access the map api key.")).ConfigureAwait(false);
                    return Unauthorized("Unsupported User.");
                }

                await _logger.Logs(new Log(0, "Info", _userhash, "GetMapApiKey", "Data", "Authorized User is given access to the map api key.")).ConfigureAwait(false);
                return Ok(_configuration["GoogleMapsApiKey"]);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        private async Task LoadUser()
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
    }
}
