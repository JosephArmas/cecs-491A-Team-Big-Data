using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.Models.ControllerModels;

namespace Utification.EntryPoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReputationController : ControllerBase
    {
        private readonly ReputationManager _reputationManager;
        private String _role;
        private String _userHash;
        private int _userID;
        private readonly IConfiguration _configuration;
        private readonly UserAccount _userAccount;
        public ReputationController(ReputationManager reputationManager, IConfiguration configuration)
        {
            _reputationManager = reputationManager;
            _configuration = configuration;
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

        [Route("GetReputation")]
        [HttpPost]

        // TODO: Change ViewCurrentReputationAsync to return DataResponse with the proper datatype for the response
        public async Task<IActionResult> GetReputationAsync([FromBody] Reports reports)
        {
            await LoadUser().ConfigureAwait(false);

            if (_role == "Anonymous User")
            {
                return Unauthorized();
            }

            var result = await _reputationManager.ViewCurrentReputationAsync(_userHash, reports.UserID).ConfigureAwait(false);
            
            if (result.IsSuccessful)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [Route("PostNewReport")]
        [HttpPost]
        public async Task<IActionResult> PostNewReportAsync([FromBody] Reports reports)
        {
            await LoadUser().ConfigureAwait(false);

            if (_role == "Anonymous User" || _userID == reports.UserID)
            {
                return Unauthorized();
            }

            Report report = new Report(reports.Rating, reports.UserID, reports.ReportingUserID, reports.Feedback);
            
            if(reports.Rating.GetType().ToString() != "System.Double" || report.Feedback.Length < 8)
            {
                return Conflict("Invalid report inputs. Please Try Again.");
            }
            var result = await _reputationManager.RecordNewUserReportAsync(_userHash, report, Convert.ToDouble(_configuration["Reputation:MinimumRoleThreshold"])).ConfigureAwait(false);

            if (!result.IsSuccessful)
            {
                IActionResult error = StatusCode(500, result.ErrorMessage);
                switch (result.ErrorMessage)
                {
                    case "Bad Request":
                        error = BadRequest(result.ErrorMessage);
                        break;
                    case "Conflict":
                        error = Conflict(result.ErrorMessage);
                        break;

                }
                return error;
            }

            return Ok(result);
        }

        [Route("ViewReports")]
        [HttpPost]
        public async Task<IActionResult> ViewReportsAsync([FromBody] Reports reports)
        {
            await LoadUser().ConfigureAwait(false);

            if(_role == "Anonymous User")
            {
                return Unauthorized();
            }
            var result = await _reputationManager.ViewUserReportsAsync(_userHash, reports.UserID, reports.ButtonCommand, reports.Partition).ConfigureAwait(false);

            if (!result.IsSuccessful)
            {
                IActionResult error = Unauthorized(result.ErrorMessage);
                switch (result.ErrorMessage)
                {
                    case "Bad Request":
                        error = BadRequest(result.ErrorMessage);
                        break;
                    case "Conflict":
                        error = Conflict(result.ErrorMessage);
                        break;
                }
                return error;
            }
            return Ok(result.Data);
        }

        [Route("GainReputation")]
        [HttpPost]
        public async Task<IActionResult> GainReputationAsync()
        {
            await LoadUser().ConfigureAwait(false);

            if (_role == "Anonymous User")
            {
                return Unauthorized("You do not have permission to do this.");
            }

            var increaseResult = await _reputationManager.IncreaseReputationByPointOneAsync(_userHash, _userID, Convert.ToDouble(_configuration["Reputation:MinimumRoleThreshold"]));

            if (!increaseResult.IsSuccessful)
            {
                return StatusCode(500, "Reputation increase failed. Please try again.");
            }

            return Ok(increaseResult.ErrorMessage);
        }

        private async Task LoadUser()
        {
            if(Request == null)
            {
                _role = "Anonymous User";
                _userHash = "";
                _userID = 0;
            }
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
            _userHash = claims.ElementAt(6).Value;
            _userID = Convert.ToInt32(claims.ElementAt(0).Value);
        }
    }
}
