using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.ErrorResponse;


namespace Utification.EntryPoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountDeletionController : Controller
    {
        private readonly DeletionManager _deletionManager;
        private readonly SecurityManager _securityManager;
        private UserAccount _userAccount;
        private UserProfile _userProfile;
        
        public AccountDeletionController()
        {
            _deletionManager = new DeletionManager;
            _securityManager = new SecurityManager();
            _userAccount = new UserAccount();
            _userProfile = new UserProfile();
        }

        [Route("health")]
        [HttpGet]
        public Task<IActionResult> HealthCheck()
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            tcs.SetResult(Ok("Working"));
            return tcs.Task;
        }
        [Route("deletes")]
        //[HttpPost]
        [HttpDelete("{user}/{del}")]
        public Task<IActionResult> DeleteAccount(UserProfile del, UserProfile user)
        {
            var tcs = new TaskCompletionSource<IActionResult>();
            var response = _deletionManager.DeleteAccount(del, user);
            tcs.SetResult(Ok(response.data));
            return tcs.Task;
        }
    }
}