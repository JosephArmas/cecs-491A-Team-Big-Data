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
        private readonly String _role;
        private readonly String _userHash;
        private readonly int _userID;
        private readonly IConfiguration _configuration;
        private readonly UserAccount _userAccount;
        public ReputationController(ReputationManager reputationManager, IConfiguration configuration)
        {
            _reputationManager = reputationManager;

            if (Request == null)
            {
                _role = "Anonymous User";
                _userHash = "";
                _userID = 0;
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
                _userHash = claims.ElementAt(6).Value;
                _userID = Convert.ToInt32(claims.ElementAt(0).Value);

            }
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
            var result = await _reputationManager.ViewCurrentReputationAsync(_userHash, reports.UserID).ConfigureAwait(false);

            if (result.IsSuccessful)
            {
                //return Ok(result.Data);
                return Unauthorized();
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
            Report report = new Report(reports.Rating, reports.UserID, reports.ReportingUserID, reports.Feedback);
            var result = await _reputationManager.RecordNewUserReportAsync(_userHash, report, Convert.ToDouble(_configuration["Reputation:MinimumRoleThreshold"])).ConfigureAwait(false);

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

            return Ok(result);
        }

        [Route("ViewReports")]
        [HttpPost]
        public async Task<IActionResult> ViewReportsAsync([FromBody] Reports reports)
        {
            Console.WriteLine("Partition: " + reports.ButtonCommand);
            var result = await _reputationManager.ViewUserReportsAsync(_userHash, reports.UserID, reports.ButtonCommand).ConfigureAwait(false);

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
    }
}
