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
    public class RecoveryController : ControllerBase
    {
        [Route("request")]
        [HttpPost]
        public Task<IActionResult> SendRequest([FromBody] RequestBody r)
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            //var manager = new SecurityManager();
            var encryptor = new Encryptor();
            var digest = encryptor.encryptString(r.newPassword);
            var response = new Response();//= manager.RecoverAccount(r.username, digest, encryptor).Result;
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
        public async Task<IActionResult> GetRequests()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            //var manager = new SecurityManager();
            var adminUser = new UserProfile(7780, "", "", "", System.DateTime.UtcNow, new GenericIdentity("Admin User"));
            var response = new Response();//= await manager.GetRecoveryRequests(adminUser);
            if (response.isSuccessful)
            {
                return Ok(response.data);
            }
            else
            {
                return Conflict(response.errorMessage);
            }
        }

        [Route("admin")]
        [HttpPost]
        //admin only
        public async Task<IActionResult> CompleteRequest([FromBody]int userID)
        {
            //var manager = new SecurityManager();
            var adminUser = new UserProfile(7780, "", "", "", System.DateTime.UtcNow, new GenericIdentity("Admin User"));
            var response = new Response();//= await manager.ResetAccount(userID, adminUser);
            if (response.isSuccessful)
            {
                return(Ok());
            }
            else
            {
                return Conflict(response.errorMessage);
            }
        }
    }
}
