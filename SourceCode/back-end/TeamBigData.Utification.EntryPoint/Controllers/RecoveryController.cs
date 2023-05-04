using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.Models.ControllerModels;
using System.Security.Principal;
using TeamBigData.Utification.Manager.Abstractions;

namespace Utification.EntryPoint.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class RecoveryController : ControllerBase
    {
        private readonly SecurityManager _securityManager;

        public RecoveryController(SecurityManager securityManager)
        {
            _securityManager = securityManager;
        }

        [Route("request")]
        [HttpPost]
        public async Task<IActionResult> SendPasswordRecoveryRequest([FromBody] RequestBody user)
        {
            // Validate user isnt logged in by the jwt

            // Decrypt user password
            // Validate user inputs


            // Make recovery request
            var response = await _securityManager.RecoverAccountPassword(user.Username, user.NewPassword, user.Userhash).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.ErrorMessage += ", {failed: _securityManager.RecoverAccount}";

                return Conflict(response.ErrorMessage);
            }
            else
            {
                return Ok(response.ErrorMessage);
            }

            /*
            var tcs = new TaskCompletionSource<IActionResult>();
            //var manager = new SecurityManager();
            var encryptor = new Encryptor();
            var digest = encryptor.encryptString(r._newPassword);
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
            */
        }

        [Route("admin")]
        [HttpGet]
        //admin only
        public async Task<IActionResult> GetRequests([FromBody] RequestBody user)
        {
            // Validate user to be admin
            var dataResponse = await _securityManager.GetRecoveryRequests(user.Userhash).ConfigureAwait(false);

            if (!dataResponse.IsSuccessful)
            {
                return Conflict(dataResponse.ErrorMessage + ", {failed: _securityManager.GetRecoveryRequests}");
            }
            else
            {
                return Ok(dataResponse.Data);
            }
            /*var tcs = new TaskCompletionSource<IActionResult>();
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
            }*/
        }

        [Route("admin")]
        [HttpPost]
        //admin only
        public async Task<IActionResult> CompleteRequest([FromBody] RequestBody body)
        {
            // Validate user
            // Validate inputs
            // Reset account
            var response = await _securityManager.ResetAccount(body.UserID,body.Userhash).ConfigureAwait(false);
            if (!response.IsSuccessful)
            {
                return Conflict(response.ErrorMessage);
            }
            else 
            { 
                return Ok(response.ErrorMessage); 
            }
            /*
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
            }*/
        }
    }
}
