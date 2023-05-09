using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.ServiceOfferingsManagers;

namespace Utification.EntryPoint.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class UserServicesController : ControllerBase
    {
        private readonly ServiceOfferingManager _serviceProviderManager;
        private readonly ServiceRequestManager _servRequestManager;
        private readonly IConfiguration _configuration;
        private String _role;
        private String _userhash;
        private int _userId;

        public UserServicesController(ServiceRequestManager servRequestManager, ServiceOfferingManager servProviderService, IConfiguration configuration)
        {
            _configuration = configuration;
            _serviceProviderManager = servProviderService;
            _servRequestManager = servRequestManager;
            _role = "Anonymous User";
            _userhash = "";
            _userId = 0;
            _configuration = configuration;
        }
        #region Service Creation
        [Route("CreateService")]
        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] ServiceModel serv)
        {
            await LoadUser().ConfigureAwait(false);
            DataResponse<int> ProvResponse = new DataResponse<int>();
            if (!InputValidation.AuthorizedUser(_role, _configuration["ServiceOfferingAuthorization:CreateService"]))
            {
                return Unauthorized("Unsupported User.");
            }

            try
            {
                ProvResponse = await _serviceProviderManager.CreateService(serv).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return Conflict("An unknown error has occured creating a service account");
            }
            if (ProvResponse.IsSuccessful == false)
            {
                return Conflict(ProvResponse.ErrorMessage);
            }
            return Ok(ProvResponse.ErrorMessage);
        }

        [Route("DeleteService")]
        [HttpPost]
        public async Task<IActionResult> DeleteService([FromBody] ServiceModel serv)
        {
            await LoadUser().ConfigureAwait(false);
            DataResponse<int> ProvResponse = new DataResponse<int>();
            var tcs = new TaskCompletionSource<IActionResult>();
            ServiceModel model = new ServiceModel();
            try
            {
                ProvResponse = await _serviceProviderManager.unregister(serv).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return Conflict("An unknown error has occured removing a service account");
            }
            return Ok(ProvResponse);
        }

        [Route("UpdateService")]
        [HttpPost]
        public async Task<IActionResult> UpdateService([FromBody] ServiceModel serv)
        {
            await LoadUser().ConfigureAwait(false);
            DataResponse<int> ProvResponse = new DataResponse<int>();
            var tcs = new TaskCompletionSource<IActionResult>();
            ServiceModel model = new ServiceModel();
            try
            {
                ProvResponse = await _serviceProviderManager.UpdateService(serv).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return Conflict("An unknown error has occured updating a service account");
            }

            return Ok(ProvResponse);
        }
        #endregion
        #region Service Requests
        [Route("CreateRequest")]
        [HttpPost]
        public async Task<IActionResult> CreateRequest([FromBody] RequestModel request)
        {
            await LoadUser().ConfigureAwait(false);
            DataResponse<int> requestresponse = new DataResponse<int>();
            try
            {
                requestresponse = await _servRequestManager.RequestService(request).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return Conflict("An unknown error has occured creating a request");
            }

            return Ok(requestresponse);
        }
        [Route("AcceptRequest")]
        [HttpPost]
        public async Task<IActionResult> AcceptRequest([FromBody] RequestModel request)
        {
            await LoadUser().ConfigureAwait(false);
            DataResponse<int> requestresponse = new DataResponse<int>();
            try
            {
                requestresponse = await _servRequestManager.AcceptRequestOffer(request).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return Conflict("An unknown error has occured accepting a request");
            }

            return Ok("Request Accepted");
        }
        [Route("DenyRequest")]
        [HttpPost]
        public async Task<IActionResult> DenyRequest([FromBody] RequestModel request)
        {
            await LoadUser().ConfigureAwait(false);
            DataResponse<int> requestresponse = new DataResponse<int>();
            try
            {
                requestresponse = await _servRequestManager.CancelRequestOffer(request, _userId).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return Conflict("An unknown error has occured denying a request");
            }
            return Ok("Request Denied");
        }
        [Route("CancelRequest")]
        [HttpPost]
        public async Task<IActionResult> CancelRequest([FromBody] RequestModel request)
        {
            await LoadUser().ConfigureAwait(false);
            Response requestresponse = new Response();
            try
            {
                requestresponse = await _servRequestManager.CancelRequest(request).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return Conflict("An unknown error has occured canceling ");
            }

            return Ok("Request Canceled");

        }
        [Route("GetRequests")]
        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {
            await LoadUser().ConfigureAwait(false);
            DataResponse<List<RequestModel>> requestresponse = new DataResponse<List<RequestModel>>();
            try
            {
                if (_role == "Service User")
                {
                    requestresponse = await _servRequestManager.GetOfferRequests(_userId).ConfigureAwait(false);
                }
                else
                {
                    requestresponse = await _servRequestManager.GetUserRequests(_userId).ConfigureAwait(false);
                }
                
            }
            catch (Exception e)
            {
                return Conflict("An unknown error has occured getting requests");
            }

            var dataResponse = JsonSerializer.Serialize<List<RequestModel>>(requestresponse.Data);
            return Ok(dataResponse);
        }
        [Route("GetServices")]
        [HttpPost]
        public async Task<IActionResult> GetServices([FromBody] RequestModel request)
        {
            await LoadUser().ConfigureAwait(false);
            DataResponse<List<ServiceModel>> requestresponse = new DataResponse<List<ServiceModel>>();
            try
            {
                requestresponse = await _servRequestManager.getservice(request).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return Conflict("An unknown error has occured getting services");
            }

            var dataResponse = JsonSerializer.Serialize<List<ServiceModel>>(requestresponse.Data);
            return Ok(dataResponse);
        }

        #endregion
        private async Task LoadUser()
        {
            const string HeaderKeyName = "HeaderKey";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues headerValue);
            HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authorizationToken);

            // Get role from JWT signature.
            string clean = authorizationToken;
            clean = clean.Remove(0, 7);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(clean);
            IEnumerable<Claim> claims = token.Claims;

            // Get whats needed from JWT.
            _role = claims.ElementAt(2).Value;
            _userhash = claims.ElementAt(6).Value;
            _userId = Convert.ToInt32(claims.ElementAt(0).Value);
        }
    }
}
