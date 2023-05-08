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
            var response = await _securityManager.RecoverAccountPassword(user.Username, user.NewPassword).ConfigureAwait(false);

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
        }

        [Route("admin/get")]
        [HttpPost]
        //admin only
        public async Task<IActionResult> GetRequests([FromBody] RequestBody user)
        {
            // Validate user to be admin
            var dataResponse = await _securityManager.GetRecoveryRequests(user.AdminID).ConfigureAwait(false);

            if (!dataResponse.IsSuccessful)
            {
                return Conflict(dataResponse.ErrorMessage + ", {failed: _securityManager.GetRecoveryRequests}");
            }
            else
            {
                return Ok(dataResponse.Data);
            }
        }

        [Route("admin/complete")]
        [HttpPost]
        //admin only
        public async Task<IActionResult> CompleteRequest([FromBody] RequestBody body)
        {
            // Validate user
            // Validate inputs
            // Reset account
            var response = await _securityManager.ResetAccount(body.Username, body.AdminID).ConfigureAwait(false);
            if (!response.IsSuccessful)
            {
                return Conflict(response.ErrorMessage);
            }
            else
            {
                return Ok(response.ErrorMessage);
            }
        }
    }
}
