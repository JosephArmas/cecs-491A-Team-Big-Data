using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.AlertManagers;
using TeamBigData.Utification.PinManagers;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using TeamBigData.Utification.Models.ControllerModels;
using TeamBigData.Utification.ErrorResponse;

namespace Utification.EntryPoint.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AlertController : ControllerBase
    {
        
        /*public AlertController()
        {

        }*/
        private readonly AlertManager _alertManager;
        private String _role;
        private String _userhash;
        private int _userId;
        private string _lat;
        private string _lng;
        private string _description;
        private int _pinType;
        private int _read;
        private string? _zipcode;
        private readonly IConfiguration _configuration;
        //private readonly ILogger _logger;
        public AlertController(AlertManager alertManager, IConfiguration configuration)
        {
            _alertManager = alertManager;
            _configuration = configuration;
            _role = "Anonymous User";
            _userhash = "";
            _userId = 0;
            _lat = "";
            _lng = "";
            _description = "";
            _pinType = 0;
            _read = 0;
            _zipcode = "";
            //_logger = logger;
        }


        //[Route("health")]
        [HttpGet("health")]
        public Task<IActionResult> HealthCheck()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            tcs.SetResult(Ok("Working"));
            return tcs.Task;
            //return Task.FromResult<IActionResult>(Ok("Working"));

        }

        //[Authorize]
        //[Route("GetAllAlerts")]
        [HttpGet("GetAllAlerts")]
        public async Task<IActionResult> GetAllAlerts()
        {
            var result = await _alertManager.GetListOfAllAlerts("userhash").ConfigureAwait(false);
            if (!result.IsSuccessful)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Failed to return a list of Alerts";
                return Conflict(result.ErrorMessage);
            }
            else
            {
                result.IsSuccessful = true;
            }

            return Ok(result.Data);
            /*
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
            if (claims.ElementAt(2).Value == "Regular User" || claims.ElementAt(2).Value == "Admin User")
            {
                return Conflict("Unauthorized User.");
            }
            var alertMan = new AlertManager();
            var result = await alertMan.GetListOfAllAlerts(claims.ElementAt(6).Value);
            if (result.isSuccessful)
            {
                return Ok(result.data);
            }
            else
            {
                return Conflict(result.errorMessage);
            }*/
        }
        //[Authorize]
        //[Route("PostNewAlert")]
        [HttpPost("PostNewAlert")]
        public async Task<IActionResult> PostNewAlert([FromBody] AlertInfo newAlert)
        {
            try
            {
                /*await LoadUser().ConfigureAwait(false);
                if (!InputValidation.AuthorizedUser(_role, _configuration["AlertAuthorization:PostNewAlert"]))
                {
                    return Unauthorized("Unsupported User.");
                }*/

                Alert alert = new Alert(newAlert.UserID, newAlert.Lat, newAlert.Lng, newAlert.Description);

                var result = await _alertManager.SaveNewAlert(alert, "temp hash").ConfigureAwait(false);
                if (!result.IsSuccessful)
                {
                    result.ErrorMessage = "Failed to save an Alert";
                    return Conflict(result.ErrorMessage);
                }
                else
                {
                    return Ok(result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            /*
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
            Alert alert = new Alert(Int32.Parse(claims.ElementAt(0).Value),newAlert._description,newAlert._dateTime,newAlert._address);
            
            var alertMan = new AlertManager();
            var response = alertMan.SaveNewAlert(alert, claims.ElementAt(6).Value);
            tcs.SetResult(Ok(response));
            return tcs.Task;*/
        }
        //[Authorize]
        //[Route("ReadUserAlert")]
        [HttpPost("ReadUserAlert")]
        public async Task<IActionResult> ReadUserAlert([FromBody] AlertInfo newAlert)
        {
            var result = await _alertManager.MarkAsRead(newAlert.AlertID, newAlert.UserID, "temp hash").ConfigureAwait(false);
            if (!result.IsSuccessful)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Failed to mark as Read";
                return Conflict(result.ErrorMessage);
            }
            else
            {
                return Ok(result.ErrorMessage);
            }
            /*
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
            if (claims.ElementAt(2).Value == "Regular User")
            {
                tcs.SetResult(Conflict("Unauthorized User."));
                return tcs.Task;
            }
            var alertMan = new AlertManager();
            var response = alertMan.MarkAsRead(newAlert._alertID, claims.ElementAt(6).Value);
            tcs.SetResult(Ok(response));
            return tcs.Task;*/
        }

        //[Authorize]
        //[Route("ModifyAlert")]
        [HttpPost("ModifyAlert")]
        public async Task<IActionResult> ModifyAlert(AlertInfo newAlert)
        {
            var result = await _alertManager.ModifyAlert(newAlert.AlertID, newAlert.UserID, newAlert.Description, "userhash").ConfigureAwait(false);
            if (!result.IsSuccessful)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Failed to modify the Alert";
                return Conflict(result.ErrorMessage);
            }
            else
            {
                return Ok(result.ErrorMessage);
            }
            /*
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
            if (claims.ElementAt(2).Value == "Regular User")
            {
                tcs.SetResult(Conflict("Unauthorized User."));
                return tcs.Task;
            }
            var alertMan = new AlertManager();
            var response = alertMan.ModifyAlert(newAlert._alertID, newAlert._description, claims.ElementAt(6).Value);
            tcs.SetResult(Ok(response));
            return tcs.Task;*/
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