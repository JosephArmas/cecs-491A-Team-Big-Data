using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public UserServicesController(ServiceRequestManager servRequestManager, ServiceOfferingManager servProviderService)
        {
            _serviceProviderManager = servProviderService;
            _servRequestManager = servRequestManager;
        }
        #region Service Creation
        [Route("CreateService")]
        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] ServiceModel serv)
        {
            Response ProvResponse = new Response();
            var tcs = new TaskCompletionSource<IActionResult>();
            ServiceModel Serv = new ServiceModel();
            ProvResponse = await _serviceProviderManager.CreateService(serv).ConfigureAwait(false);

            //return (IActionResult)tcs;
            return Ok(ProvResponse.ErrorMessage);
        }

        [Route("DeleteService")]
        [HttpPost]
        public async Task<IActionResult> DeleteService([FromBody] ServiceModel serv)
        {
            Response ProvResponse = new Response();
            var tcs = new TaskCompletionSource<IActionResult>();
            ServiceModel model = new ServiceModel();
            ProvResponse = await _serviceProviderManager.unregister(serv).ConfigureAwait(false);
            return Ok(ProvResponse.ErrorMessage);
        }

        [Route("UpdateService")]
        [HttpPost]
        public async Task<IActionResult> UpdateService([FromBody] ServiceModel serv)
        {
            Response ProvResponse = new Response();
            var tcs = new TaskCompletionSource<IActionResult>();
            ServiceModel model = new ServiceModel();
            ProvResponse = await _serviceProviderManager.UpdateService(serv).ConfigureAwait(false);
            return Ok(ProvResponse.ErrorMessage);
        }
        #endregion
        #region Service Requests
        [Route("CreateRequest")]
        [HttpPost]
        public async Task<IActionResult> CreateRequest([FromBody] RequestModel request)
        {
            return Ok(request.Distance);
        }
        [Route("AcceptRequest")]
        [HttpPost]
        public async Task<IActionResult> AcceptRequest()
        {
            return Ok("Request Accepted");
        }
        [Route("DenyRequest")]
        [HttpPost]
        public async Task<IActionResult> DenyRequest()
        {
            return Ok("Request Denied");
        }
        [Route("CancelRequest")]
        [HttpPost]
        public async Task<IActionResult> CancelRequest()
        {
            List<RequestModel> jsonlist = new List<RequestModel>();
            RequestModel request = new RequestModel();
            request.RequestID = 3;
            request.ServiceName = "Ager";
            request.Accept = 2;
            request.RequestLat = "0.0";
            request.RequestLong = "0.0";
            request.Requester = 4;
            request.Distance = 3;
            request.PinType = 1;
            request.ServiceID = 8;
            RequestModel request2 = new RequestModel();
            request2.RequestID = 3;
            request2.ServiceName = "TiresAreGone";
            request2.Accept = 2;
            request2.RequestLat = "5.0";
            request2.RequestLong = "0.0";
            request2.Requester = 6;
            request2.Distance = 7;
            request2.PinType = 1;
            request2.ServiceID = 8;
            var jsonstr = JsonSerializer.Serialize<RequestModel>(request);
            var jsonstr2 = JsonSerializer.Serialize<RequestModel>(request2);
            jsonlist.Add(request);
            jsonlist.Add(request2);
            var jsonstr3 = JsonSerializer.Serialize<List<RequestModel>>(jsonlist);
            return Ok(jsonstr3);

        }
        [Route("GetRequests")]
        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {

            return Ok("Requests Gotten");
        }
        [Route("GetServices")]
        [HttpGet]
        public async Task<IActionResult> GetServices()
        {
            return Ok("Services Gotten");
        }

        #endregion
    }
}
