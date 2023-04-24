using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.UserServicesManager;

namespace Utification.EntryPoint.Controllers
{
    [BindProperties]
    public class ServInfo
    {
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public string ServicePhone { get; set; }
        public string? ServiceURL { get; set; }
        public int? ServiceID { get; set; }
        public string? ServiceLat { get; set; }
        public string? ServiceLong { get; set; }
        public int PinTypes { get; set; }
        public int Distance { get; set; }
        public int CreatedBy { get; set; }

    }
    [BindProperties]
    public class RequestInfo
    {
        public int? RequestID { get; set; }
        public int ServiceID { get; set; }
        public string? ServiceName { get; set; }
        public int? Requester { get; set; }
        public string? RequestLat { get; set; }
        public string? RequestLong { get; set; }
        public int? PinType { get; set; }
        public int? Accept { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class UserServicesController : ControllerBase
    {
        private ServModel ServBind(ServInfo info, ServModel model)
        {
            model.ServiceName = info.ServiceName;
            model.ServiceDescription = info.ServiceDescription;
            model.ServicePhone = info.ServicePhone;
            model.ServiceURL = info.ServiceURL;
            model.ServiceLat = info.ServiceLat;
            model.ServiceLong = info.ServiceLong;
            model.PinTypes = info.PinTypes;
            model.Distance = info.Distance;
            model.CreatedBy = info.CreatedBy;
            return model;
        }


        #region Service Creation
        [Route("CreateService")]
        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] ServInfo serv)
        {
            Response ProvResponse = new Response();
            ServiceProviderManager ServManager = new ServiceProviderManager();
            ServModel Serv = new ServModel();

            Serv = ServBind(serv,Serv);

            try
            {
                ProvResponse = await ServManager.CreateService(Serv).ConfigureAwait(false);
            }
            catch(Exception e)
            {
                ProvResponse.isSuccessful = false;
                ProvResponse.errorMessage += "Service Creation Failed";
            }
            return Ok(ProvResponse.errorMessage);
        }

        [Route("DeleteService")]
        [HttpPost]
        public async Task<IActionResult> DeleteService([FromBody] ServInfo serv)
        {
            Response ProvResponse = new Response();
            ServiceProviderManager ServManager = new ServiceProviderManager();
            ServModel model = new ServModel();

            model = ServBind(serv, model);
            try
            {
                ProvResponse = await ServManager.unregister(model).ConfigureAwait(false);
            }
            catch(Exception e)
            {
                ProvResponse.isSuccessful = false;
                ProvResponse.errorMessage += "Service Deletion Failed";
            }
            return Ok(ProvResponse.errorMessage);
        }

        [Route("UpdateService")]
        [HttpPost]
        public async Task<IActionResult> UpdateService([FromBody] ServInfo serv)
        {
            Response ProvResponse = new Response();
            ServiceProviderManager ServManager = new ServiceProviderManager();
            ServModel model = new ServModel();

            model = ServBind(serv, model);

            try
            {
                ProvResponse = await ServManager.UpdateService(model).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                ProvResponse.isSuccessful = false;
                ProvResponse.errorMessage += "Service Update Failed";
            }
            return Ok(ProvResponse.errorMessage);
        }
        #endregion
        #region Service Requests
        [Route("CreateRequest")]
        [HttpPost]
        public async Task<IActionResult> CreateRequest()
        {
            return Ok();
        }
        [Route("AcceptRequest")]
        [HttpPost]
        public async Task<IActionResult> AcceptRequest()
        {
            return Ok();
        }
        [Route("CancelRequest")]
        [HttpPost]
        public async Task<IActionResult> CancelRequest()
        {
            return Ok();
        }
        [Route("GetRequests")]
        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {
            return Ok();
        }
        [Route("GetServices")]
        [HttpGet]
        public async Task<IActionResult> GetServices()
        {
            return Ok();
        }

        #endregion
    }
}
