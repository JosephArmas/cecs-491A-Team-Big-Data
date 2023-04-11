using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using System.Security.Principal;

namespace Utification.EntryPoint.Controllers
{
    public class RequestBody
    {
        public String username { get; set; }
        public String newPassword { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class RecoveryController : Controller
    {
        [Route("health")]
        [HttpGet]
        public Task<IActionResult> HealthCheck()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            tcs.SetResult(Ok("Working"));
            return tcs.Task;
        }

        [Route("request")]
        [HttpPost]
        public Task<IActionResult> SendRequest([FromBody] RequestBody r)
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            var manager = new SecurityManager();
            var encryptor = new Encryptor();
            var digest = encryptor.encryptString(r.newPassword);
            var response = manager.RecoverAccount(r.username, digest, encryptor).Result;
            if (response.isSuccessful)
            {
                tcs.SetResult(Ok());
            }
            else
            {
                tcs.SetResult(Conflict(response.errorMessage));
            }
            return tcs.Task;
        }

        [Route("admin")]
        [HttpGet]
        //admin only
        public Task<IActionResult> GetRequests()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            var manager = new SecurityManager();
            var requests = new List<UserProfile>();
            var adminUser = new UserProfile(7780, "", "", "", System.DateTime.UtcNow, new GenericIdentity("Admin User"));
            var response = manager.GetRecoveryRequests(ref requests, adminUser);
            if(response.isSuccessful)
            {
                tcs.SetResult(Ok(requests));
            }
            else
            {
                tcs.SetResult(Conflict(response.errorMessage));
            }
            return tcs.Task;
        }

        [Route("admin")]
        [HttpPost]
        //admin only
        public Task<IActionResult> CompleteRequest([FromBody]int userID)
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            var manager = new SecurityManager();
            var adminUser = new UserProfile(7780, "", "", "", System.DateTime.UtcNow, new GenericIdentity("Admin User"));
            var response = manager.ResetAccount(userID, adminUser);
            if(response.isSuccessful)
            {
                tcs.SetResult(Ok());
            }
            else
            {
                tcs.SetResult(Conflict(response.errorMessage));
            }
            return tcs.Task;
        }
    }
}
