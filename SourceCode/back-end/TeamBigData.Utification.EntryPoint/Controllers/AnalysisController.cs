using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.AnalysisManagers;

namespace Utification.EntryPoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnalysisController : ControllerBase
    {
        [Route("logins")]
        [HttpGet]
        public Task<IActionResult> GetLogins()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            var manager = new AnalysisManager();
            //var rows = manager.GetLogins().Result;
            //tcs.SetResult(Ok(rows));
            return tcs.Task;
        }

        [Route("registrations")]
        [HttpGet]
        public Task<IActionResult> GetRegistrations()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            var manager = new AnalysisManager();
            //var rows = manager.GetRegistrations().Result;
            //tcs.SetResult(Ok(rows));
            return tcs.Task;
        }

        [Route("pins")]
        [HttpGet]
        public Task<IActionResult> GetPinsAdded()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            var manager = new AnalysisManager();
            //var rows = manager.GetPinsAdded().Result;
            //tcs.SetResult(Ok(rows));
            return tcs.Task;
        }

        [Route("maps")]
        [HttpGet]
        public Task<IActionResult> GetPinPulls()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            var manager = new AnalysisManager();
            //var rows = manager.GetPinPulls().Result;
            // tcs.SetResult(Ok(rows));
            return tcs.Task;
        }
    }
}
