using System.Collections;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.UserServices;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;

namespace TeamBigData.Utification.ServiceOfferingsServices
{
    public class ServiceRequestService
    {

        private readonly IServicesDBInserter _servicesDBInserter;
        private readonly IServicesDBSelecter _servicesDBSelecter;
        private readonly IServicesDBUpdater _servicesDBUpdater;

        public ServiceRequestService(ServicesSqlDAO servicesSqlDAO)
        {
            _servicesDBInserter = servicesSqlDAO;
            _servicesDBSelecter = servicesSqlDAO;
            _servicesDBUpdater = servicesSqlDAO;
        }

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<Response> AcceptRequestOffer(RequestModel request)
        {
            /*var result = await _servicesDBUpdater.UpdateRequestAccept(request).ConfigureAwait(false);
            if ((int)result.data == 0)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Failed to accept request";
            }
            else if ((int)result.data > 1)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Accepted Request, but there were multiple accepts";
            }
            else
            {
                result.IsSuccessful = true;
                result.ErrorMessage = "Successfully accepted request";
            }
            return result;*/

            throw new NotImplementedException();
        }
        public async Task<Response> CancelRequest()
        {
            throw new NotImplementedException();
        }

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<Response> CancelRequestOffer(RequestModel request)
        {
            /*var result = await _servicesDBUpdater.UpdateRequestDeny(request).ConfigureAwait(false);
            if ((int)result.data == 1)
            {
                result.IsSuccessful = true;
                result.ErrorMessage = "Successfully updated request";
            }
            else
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Failed to cancel request";
            }
            return result;*/

            throw new NotImplementedException();
        }

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<Response> RequestService(ServiceModel company, Pin pin)
        {
            /*var result = await _servicesDBInserter.InsertServiceReq(company, pin).ConfigureAwait(false);
            if ((int)result.data == 0)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Failed to insert request";
            }
            else
            {
                result.IsSuccessful = true;
                result.ErrorMessage = "Successfully entered request";
            }
            return result;*/

            throw new NotImplementedException();
        }

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<Response> GetOfferRequests(ServiceModel serv)
        {
            /*var result = await _servicesDBSelecter.GetProviderRequests(serv).ConfigureAwait(false);
            List<RequestModel> provrequests = new List<RequestModel>();
            List<ArrayList> requests = (List<ArrayList>)result.data;
            foreach (var item in requests)
            {
                RequestModel requestModel = new RequestModel() { RequestID = (int)item[0], ServiceID = (int)item[1], ServiceName = (string)item[2], Requester = (int)item[3], RequestLat = (string)item[4], RequestLong = (string)item[5], PinType = (int)item[6], Accept = (int)item[7] };
                if (requestModel.ServiceID != serv.ServiceID || requestModel.ServiceName != serv.ServiceName)
                {
                    result.ErrorMessage = "Failed to get correct service requests";
                    result.IsSuccessful = false;
                    return result;
                }
                provrequests.Add(requestModel);
            }
            result.IsSuccessful = true;
            result.data = provrequests;
            return result;*/

            throw new NotImplementedException();
        }

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<Response> GetUserRequests(UserProfile user)
        {
            /*
            var result = await _servicesDBSelecter.GetUserRequests(user).ConfigureAwait(false);
            List<RequestModel> provrequests = new List<RequestModel>();
            List<ArrayList> requests = (List<ArrayList>)result.data;
            foreach (var item in requests)
            {
                RequestModel requestModel = new RequestModel() { RequestID = (int)item[0], ServiceID = (int)item[1], ServiceName = (string)item[2], Requester = (int)item[3], RequestLat = (string)item[4], RequestLong = (string)item[5], PinType = (int)item[6], Accept = (int)item[7] };
                if (user.UserID != requestModel.Requester)
                {
                    result.ErrorMessage = "Failed to get correct service requests";
                    result.IsSuccessful = false;
                    return result;
                }
                provrequests.Add(requestModel);
            }
            result.IsSuccessful = true;
            result.data = provrequests;
            return result;*/

            throw new NotImplementedException();
        }

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<Response> getservice(Pin pin, int dist)
        {
            /*
            var result = await _servicesDBSelecter.getnearbyservice(pin, dist).ConfigureAwait(false);
            List<ServiceModel> servicelist = new List<ServiceModel>();
            List<ArrayList> services = (List<ArrayList>)result.data;
            foreach (var item in services)
            {
                ServiceModel servModel = new ServiceModel() { ServiceName = (string)item[0], ServiceDescription = (string)item[1], ServicePhone = (string)item[2], ServiceURL = (string)item[3], ServiceID = (int)item[4], ServiceLat = (string)item[5], ServiceLong = (string)item[6], PinTypes = (int)item[7], Distance = (int)item[8] };
                servicelist.Add(servModel);
            }
            result.IsSuccessful = true;
            result.data = servicelist;
            return result;*/

            throw new NotImplementedException();
        }
    }
}