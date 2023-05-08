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

        public async Task<Response> AcceptRequestOffer(RequestModel request)
        {
            var requestresponse = new Response();

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
        public async Task<Response> CancelRequestOffer(RequestModel request)
        {
            var requestresponse = new Response();
            if (request == null)
            {
                requestresponse.IsSuccessful = false;
                requestresponse.ErrorMessage = "Manager RequestModel parameter is null";
                return requestresponse;
            }

            requestresponse = await _servService.CancelRequestOffer(request).ConfigureAwait(false);

            return requestresponse;

        }
        public async Task<Response> getservice(Pin pin, int dist)
        {
            var requestresponse = new Response();

            if (pin == null || dist == null)
            {
                requestresponse.IsSuccessful = false;
                requestresponse.ErrorMessage = "Manager parameter is null";
                return requestresponse;
            }

            if (dist < 1 || dist > 25)
            {
                requestresponse.IsSuccessful = false;
                requestresponse.ErrorMessage = "Distance is outside of permitted area";
                return requestresponse;
            }

            requestresponse = await _servService.getservice(pin, dist).ConfigureAwait(false);

            return requestresponse;
        }
        public async Task<Response> RequestService(ServiceModel company, Pin pin)
        {
            var requestresponse = new Response();

            if (company == null || pin == null)
            {
                requestresponse.IsSuccessful = false;
                requestresponse.ErrorMessage = "Manager parameter is null";
                return requestresponse;
            }

            requestresponse = await _servService.RequestService(company, pin).ConfigureAwait(false);

            return requestresponse;
        }
        public async Task<Response> GetOfferRequests(ServiceModel serv)
        {
            var requestresponse = new Response();

            if (serv == null)
            {
                requestresponse.IsSuccessful = false;
                requestresponse.ErrorMessage = "Manager ServiceModel parameter is null";
                return requestresponse;
            }

            requestresponse = await _servService.GetOfferRequests(serv).ConfigureAwait(false);

            return requestresponse;
        }
        public async Task<Response> GetUserRequests(UserProfile user)
        {
            var requestresponse = new Response();

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