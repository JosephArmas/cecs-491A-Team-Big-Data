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
        public async Task<IActionResult> GetReports()
        {
            var result = await _reputationManager.ViewUserReportsAsync().ConfigureAwait(false);
            return Ok(result);
        }

        [Route("PostNewReport")]
        [HttpPost]
        public async Task<IActionResult> PostNewReport()
        {
            var result = await _reputationManager.RecordNewUserReportAsync(4.2).ConfigureAwait(false);
            return Ok(result);
        }


    }
}
