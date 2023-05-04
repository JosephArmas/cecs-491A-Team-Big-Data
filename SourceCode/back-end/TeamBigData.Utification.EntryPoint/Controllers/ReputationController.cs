using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.Models.ControllerModels;
using TeamBigData.Utification.ReputationManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public async Task<IActionResult> GetReputationAsync([FromBody] Report reports)
        {
            var result = await _reputationManager.ViewCurrentReputationAsync(reports.UserID).ConfigureAwait(false);

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
        public async Task<IActionResult> PostNewReportAsync()
        {
            var result = await _reputationManager.RecordNewUserReportAsync(4.2).ConfigureAwait(false);

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
        public async Task<IActionResult> ViewReportsAsync()
        {
            var result = await _reputationManager.ViewUserReportsAsync().ConfigureAwait(false);

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
            return Ok(result.ErrorMessage);
        }
    }
}
