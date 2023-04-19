using Microsoft.AspNetCore.Mvc;

namespace Utification.EntryPoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReputationController : ControllerBase
    {

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

        public Task<IActionResult> ViewReports()
        {
            var tcs = new TaskCompletionSource<IActionResult>();

            tcs.SetResult(Ok("Healthy"));
            return tcs.Task;
        }
    }
}
