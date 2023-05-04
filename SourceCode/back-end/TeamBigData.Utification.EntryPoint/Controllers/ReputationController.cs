using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetReputationAsync([FromBody] Reports reports)
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
        public async Task<IActionResult> PostNewReportAsync([FromBody] Reports reports)
        {
            Report report = new Report(reports.Rating, reports.UserID, reports.ReportingUserID, reports.Feedback);
            var result = await _reputationManager.RecordNewUserReportAsync(report, 4.2).ConfigureAwait(false);

            if (!result.IsSuccessful)
            {
                IActionResult error = Unauthorized(result.ErrorMessage);
                switch(result.ErrorMessage)
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
            var result = await _reputationManager.ViewUserReportsAsync(reports.UserID, reports.ButtonCommand).ConfigureAwait(false);  
            
            if(!result.isSuccessful)
            {
                IActionResult error = Unauthorized(result.errorMessage);
                switch(result.errorMessage)
                {
                    case "Bad Request":
                        error = BadRequest(result.errorMessage);
                        break;
                    case "Conflict":
                        error = Conflict(result.errorMessage);
                        break;
                }
                return error;
            }
            return Ok(result.data);
        }
    }
}
