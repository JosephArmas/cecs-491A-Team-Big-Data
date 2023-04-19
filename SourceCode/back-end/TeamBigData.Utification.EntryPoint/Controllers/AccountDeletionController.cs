using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.DeletionManager;
using TeamBigData.Utification.ErrorResponse;


namespace Utification.EntryPoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountDeletionController : ControllerBase
    {
        private readonly DeletionManager _deletionManager;
        private readonly SecurityManager _securityManager;
        private UserAccount _userAccount;
        private UserProfile _userProfile;

        public AccountDeletionController()
        {
            _deletionManager = new DeletionManager();
            _securityManager = new SecurityManager();
            _userAccount = new UserAccount();
            _userProfile = new UserProfile();
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
        [Route("deletes")]
        //[HttpPost]
        //[HttpDelete("{user}/{del}")]
        [HttpDelete("deletes/{del}/{user}")]
        public async Task<IActionResult> DeleteAccount([FromRoute]UserProfile del, [FromRoute]UserProfile user)
        {
            //var tcs = new TaskCompletionSource<IActionResult>();
            var response = await _deletionManager.DeleteAccount(del, user).ConfigureAwait(false);
            //tcs.SetResult(Ok(response.data));
            return Ok(response);
        }
    }
}