using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.AnalysisManagers;

namespace Utification.EntryPoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnalysisController : ControllerBase
    {
        private readonly AnalysisManager manager;
        public AnalysisController(AnalysisManager Amanager)
        {
            manager = Amanager;
        }

        [Route("logins")]
        [HttpGet]
        public Task<IActionResult> GetLogins()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            var rows = manager.GetLogins().Result;
            tcs.SetResult(Ok(rows.Data));
            return tcs.Task;
        }

        [Route("registrations")]
        [HttpGet]
        public Task<IActionResult> GetRegistrations()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            var rows = manager.GetRegistrations().Result;
            tcs.SetResult(Ok(rows.Data));
            return tcs.Task;
        }

        [Route("pins")]
        [HttpGet]
        public Task<IActionResult> GetPinsAdded()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            var rows = manager.GetPinsAdded().Result;
            tcs.SetResult(Ok(rows.Data));
            return tcs.Task;
        }

        [Route("maps")]
        [HttpGet]
        public Task<IActionResult> GetPinPulls()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            var rows = manager.GetPinPulls().Result;
             tcs.SetResult(Ok(rows.Data));
            return tcs.Task;
        }
    }
}
