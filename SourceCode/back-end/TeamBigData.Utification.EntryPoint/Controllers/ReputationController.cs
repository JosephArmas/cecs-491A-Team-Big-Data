using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.Manager;

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
        [HttpGet]
        public async Task<IActionResult> GetReportsAsync()
        {
            var result = await _reputationManager.ViewCurrentReputationAsync().ConfigureAwait(false);

            if (result.isSuccessful)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        //Check the flag. 

        [Route("PostNewReport")]
        [HttpPost]
        public async Task<IActionResult> PostNewReportAsync()
        {
            var result = await _reputationManager.RecordNewUserReportAsync(4.2).ConfigureAwait(false);

            if (result.errorMessage != null)
            {
                IActionResult error = Unauthorized(result.errorMessage);
                switch(result.errorMessage)
                {
                    case "Bad Request":
                        error = BadRequest(result);
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
            var result = _reputationManager.ViewUserReportsAsync().ConfigureAwait(false);     
            return Ok(result);
        }
    }
}
