using System.Collections;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.UserServices;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;

namespace TeamBigData.Utification.ServiceOfferingsServices
{
    public class ServiceRequestService
    {

        private readonly IServicesDBInserter _servicesDBInserter;
        private readonly IServicesDBSelecter _servicesDBSelecter;
        private readonly IServicesDBUpdater _servicesDBUpdater;

        public ServiceRequestService(IServicesDBInserter servicesDBInserter, IServicesDBSelecter servicesDBSelecter, IServicesDBUpdater servicesDBUpdater)
        {
            _servicesDBInserter = servicesDBInserter;
            _servicesDBSelecter = servicesDBSelecter;
            _servicesDBUpdater = servicesDBUpdater;
        }
        public async Task<Response> AcceptRequestOffer(RequestModel request)
        {
            var result = await _servicesDBUpdater.UpdateRequestAccept(request).ConfigureAwait(false);
            if ((int)result.data == 0)
            {
                result.isSuccessful = false;
                result.errorMessage = "Failed to accept request";
            }
            else if ((int)result.data > 1)
            {
                result.isSuccessful = false;
                result.errorMessage = "Accepted Request, but there were multiple accepts";
            }
            else
            {
                result.isSuccessful = true;
                result.errorMessage = "Successfully accepted request";
            }
            return result;
        }
        public async Task<Response> CancelRequest()
        {
            throw new NotImplementedException();
        }
        public async Task<Response> CancelRequestOffer(RequestModel request)
        {
            var result = await _servicesDBUpdater.UpdateRequestDeny(request).ConfigureAwait(false);
            if ((int)result.data == 1)
            {
                result.isSuccessful = true;
                result.errorMessage = "Successfully updated request";
            }
            else
            {
                result.isSuccessful = false;
                result.errorMessage = "Failed to cancel request";
            }
            return result;
        }
        public async Task<Response> RequestService(ServiceModel company, Pin pin)
        {
            var result = await _servicesDBInserter.InsertServiceReq(company, pin).ConfigureAwait(false);
            if ((int)result.data == 0)
            {
                result.isSuccessful = false;
                result.errorMessage = "Failed to insert request";
            }
            else
            {
                result.isSuccessful = true;
                result.errorMessage = "Successfully entered request";
            }
            return result;
        }
        public async Task<Response> GetOfferRequests(ServiceModel serv)
        {
            var result = await _servicesDBSelecter.GetProviderRequests(serv).ConfigureAwait(false);
            List<RequestModel> provrequests = new List<RequestModel>();
            List<ArrayList> requests = (List<ArrayList>)result.data;
            foreach (var item in requests)
            {
                RequestModel requestModel = new RequestModel() { RequestID = (int)item[0], ServiceID = (int)item[1], ServiceName = (string)item[2], Requester = (int)item[3], RequestLat = (string)item[4], RequestLong = (string)item[5], PinType = (int)item[6], Accept = (int)item[7] };
                if (requestModel.ServiceID != serv.ServiceID || requestModel.ServiceName != serv.ServiceName)
                {
                    result.errorMessage = "Failed to get correct service requests";
                    result.isSuccessful = false;
                    return result;
                }
                provrequests.Add(requestModel);
            }
            result.isSuccessful = true;
            result.data = provrequests;
            return result;
        }
        public async Task<Response> GetUserRequests(UserProfile user)
        {
            var result = await _servicesDBSelecter.GetUserRequests(user).ConfigureAwait(false);
            List<RequestModel> provrequests = new List<RequestModel>();
            List<ArrayList> requests = (List<ArrayList>)result.data;
            foreach (var item in requests)
            {
                RequestModel requestModel = new RequestModel() { RequestID = (int)item[0], ServiceID = (int)item[1], ServiceName = (string)item[2], Requester = (int)item[3], RequestLat = (string)item[4], RequestLong = (string)item[5], PinType = (int)item[6], Accept = (int)item[7] };
                if (user._userID != requestModel.Requester)
                {
                    result.errorMessage = "Failed to get correct service requests";
                    result.isSuccessful = false;
                    return result;
                }
                provrequests.Add(requestModel);
            }
            result.isSuccessful = true;
            result.data = provrequests;
            return result;
        }
        public async Task<Response> getservice(Pin pin, int dist)
        {
            var result = await _servicesDBSelecter.getnearbyservice(pin, dist).ConfigureAwait(false);
            List<ServiceModel> servicelist = new List<ServiceModel>();
            List<ArrayList> services = (List<ArrayList>)result.data;
            foreach (var item in services)
            {
                ServiceModel servModel = new ServiceModel() { ServiceName = (string)item[0], ServiceDescription = (string)item[1], ServicePhone = (string)item[2], ServiceURL = (string)item[3], ServiceID = (int)item[4], ServiceLat = (string)item[5], ServiceLong = (string)item[6], PinTypes = (int)item[7], Distance = (int)item[8] };
                servicelist.Add(servModel);
            }
            result.isSuccessful = true;
            result.data = servicelist;
            return result;
        }
    }
}