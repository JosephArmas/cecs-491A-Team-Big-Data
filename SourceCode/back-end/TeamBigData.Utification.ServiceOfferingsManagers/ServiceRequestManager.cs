using Azure.Core;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.ServiceOfferingsServices;

namespace TeamBigData.Utification.ServiceOfferingsManagers
{
    public class ServiceRequestManager
    {
        private readonly ServiceRequestService _servService;
        private readonly ILogger _logger;
        public ServiceRequestManager(ServiceRequestService servService, ILogger logger)
        {
            _servService = servService;
            _logger = logger;
        }
        Response ReqResponse = new Response();

        public async Task<DataResponse<int>> AcceptRequestOffer(RequestModel request)
        {
            var requestresponse = new DataResponse<int>();

            if (request == null)
            {
                requestresponse.IsSuccessful = false;
                requestresponse.ErrorMessage = "Manager RequestModel parameter is null";
                return requestresponse;
            }

            requestresponse = await _servService.AcceptRequestOffer(request).ConfigureAwait(false);

            return requestresponse;
        }
        public async Task<Response> CancelRequest(RequestModel request)
        {
            var requestresponse = new Response();
            if (request == null)
            {
                requestresponse.IsSuccessful = false;
                requestresponse.ErrorMessage = "Manager RequestModel parameter is null";
                return requestresponse;
            }

            requestresponse = await _servService.CancelRequest().ConfigureAwait(false);

            return requestresponse;

        }
        public async Task<DataResponse<int>> CancelRequestOffer(RequestModel request, int userid)
        {
            var requestresponse = new DataResponse<int>();
            if (request == null)
            {
                requestresponse.IsSuccessful = false;
                requestresponse.ErrorMessage = "Manager RequestModel parameter is null";
                return requestresponse;
            }

            requestresponse = await _servService.CancelRequestOffer(request, userid).ConfigureAwait(false);

            return requestresponse;

        }
        public async Task<DataResponse<List<ServiceModel>>> getservice(RequestModel request)
        {
            var requestresponse = new DataResponse<List<ServiceModel>>();

            if (request == null)
            {
                requestresponse.IsSuccessful = false;
                requestresponse.ErrorMessage = "Manager parameter is null";
                return requestresponse;
            }

            if (request.Distance < 1 || request.Distance > 25)
            {
                requestresponse.IsSuccessful = false;
                requestresponse.ErrorMessage = "Distance is outside of permitted area";
                return requestresponse;
            }
            requestresponse = await _servService.getservice(request).ConfigureAwait(false);

            return requestresponse;
        }
        public async Task<DataResponse<int>> RequestService(RequestModel request)
        {
            var requestresponse = new DataResponse<int>();

            if (request == null)
            {
                requestresponse.IsSuccessful = false;
                requestresponse.ErrorMessage = "Manager parameter is null";
                return requestresponse;
            }

            requestresponse = await _servService.RequestService(request).ConfigureAwait(false);

            return requestresponse;
        }
        public async Task<DataResponse<List<RequestModel>>> GetOfferRequests(int serv)
        {
            var requestresponse = new DataResponse<List<RequestModel>>();

            if (serv == null)
            {
                requestresponse.IsSuccessful = false;
                requestresponse.ErrorMessage = "Manager ServiceModel parameter is null";
                return requestresponse;
            }

            requestresponse = await _servService.GetOfferRequests(serv).ConfigureAwait(false);

            return requestresponse;
        }
        public async Task<DataResponse<List<RequestModel>>> GetUserRequests(int user)
        {
            var requestresponse = new DataResponse<List<RequestModel>>();

            if (user == null)
            {
                requestresponse.IsSuccessful = false;
                requestresponse.ErrorMessage = "Manager UserProfile parameter is null";
                return requestresponse;
            }

            requestresponse = await _servService.GetUserRequests(user).ConfigureAwait(false);

            return requestresponse;
        }
    }
}