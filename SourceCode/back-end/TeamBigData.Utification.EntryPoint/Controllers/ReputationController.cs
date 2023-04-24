using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using Utification.EntryPoint.Models;

namespace Utification.EntryPoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReputationController : ControllerBase
    {
        private readonly ReputationManager _reputationManager;
        public ReputationController(ReputationManager reputationManager) 
        {
            _reputationManager = reputationManager;
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
        public async Task<IActionResult> GetReputationAsync([FromBody]UserProfileInfo newProfile)
        {
            UserProfile userProfile = new UserProfile(newProfile.UserID);

            var result = await _reputationManager.ViewCurrentReputationAsync(userProfile).ConfigureAwait(false);

            if (result.isSuccessful)
            {
                return Ok(result.data);
            }
            else
            {
                return BadRequest("Reputation is Unavilable Right Now. Please Try Again");
            }
        }

        [Route("PostNewReport")]
        [HttpPost]
        public async Task<IActionResult> PostNewReportAsync()
        {
            ReportInfo reportInfo = new ReportInfo();

            Report report = new Report(reportInfo.Rating, reportInfo.ReportedUser, reportInfo.ReportingUser, reportInfo.Feedback);

            var result = await _reputationManager.RecordNewUserReportAsync(report, 4.2).ConfigureAwait(false);

            if (result.errorMessage != null)
            {
                IActionResult error = Unauthorized(result.errorMessage);
                switch(result.errorMessage)
                {
                    case "Please submit a valid report":
                        error = BadRequest(result.errorMessage);
                        break;

                    case "Report Failed to Submit. Please Try Again.":
                        error = Conflict(result.errorMessage);
                        break;
                }
                return error;
            }
            else
            {
                return Ok(result);
            }
        }

        [Route("ViewReports")]
        public async Task<IActionResult> ViewReportsAsync()
        {
            var result = await _reputationManager.ViewUserReportsAsync().ConfigureAwait(false);

            if (result.isSuccessful)
            {
                return Ok(result.data);
            }
            else
            {
                result.errorMessage = "Reputation is Unavailable Right Now. Please Try Again.";
                return BadRequest(result.errorMessage);
            }
        }
    }
}
