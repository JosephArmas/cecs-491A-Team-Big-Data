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

namespace Utification.EntryPoint.Controllers
{
    [BindProperties]
    public class AlertInfo
    {
        public int _alertID { get; set; }
        public int _userID { get; set; }
        public string _lat { get; set; }
        public string _lng { get; set; }
        public string _description { get; set; }
        public int _read { get; set; }
        public string _dateTime { get; set; }
        public string _zipcode { get; set; }
    }
    [ApiController]
    [Route("[controller]")]
    public class AlertController : ControllerBase
    {
        public AlertController()
        {

        }
        private readonly AlertManager _alertManager;
        public AlertController(AlertManager alertManager)
        {
            _alertManager = alertManager;
        }
        [Authorize]
        [Route("GetAllAlerts")]
        [HttpGet]
        public async Task<IActionResult> GetAllAlerts()
        {
            var result = await _alertManager.GetListOfAllAlerts("userhash").ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                result.isSuccessful = false;
                result.errorMessage = "Failed to return a list of Alerts";
                return Conflict(result.errorMessage);
            }
            else
            {
                result.isSuccessful = true;
            }

            return Ok(result.data);
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
        [Authorize]
        [Route("PostNewAlert")]
        [HttpPost]
        public async Task<IActionResult> PostNewAlert([FromBody] AlertInfo newAlert)
        {
            Alert alert = new Alert(newAlert._userID, newAlert._lat, newAlert._lng, newAlert._description);

            var result = await _alertManager.SaveNewAlert(alert,"temp hash").ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                result.errorMessage = "Failed to save an Alert";
                return Conflict(result.errorMessage);
            }
            else
            {
                return Ok(result.errorMessage);
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
        [Authorize]
        [Route("ReadUserAlert")]
        [HttpPost]
        public async Task<IActionResult> ReadUserAlert([FromBody]AlertInfo newAlert)
        {
            var result = await _alertManager.MarkAsRead(newAlert._alertID, newAlert._userID, "temp hash").ConfigureAwait(false);
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
        
        [Authorize]
        [Route("ModifyAlert")]
        [HttpPost]
        public async Task<IActionResult> ModifyAlert(AlertInfo newAlert)
        {
            var result = await _alertManager.ModifyAlert(newAlert._alertID, newAlert._userID, newAlert._description, "userhash").ConfigureAwait(false);
            if (!result.isSuccessful)
            {
                result.isSuccessful = false;
                result.errorMessage += ", {failed: _pinManager.ChangePinContent}";
                return Conflict(result.errorMessage);
            }
            else
            {
                return Ok(result.errorMessage);
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

    }
}