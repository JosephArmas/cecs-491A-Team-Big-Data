using System.Collections;
using System.Collections.Generic;
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
        public async Task<DataResponse<int>> AcceptRequestOffer(RequestModel request)
        {
            var result = await _servicesDBUpdater.UpdateRequestAccept(request).ConfigureAwait(false);
            if (result.Data == 0)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Failed to accept request";
            }
            else if (result.Data > 1)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Accepted Request, but there were multiple accepts";
            }
            else
            {
                result.IsSuccessful = true;
                result.ErrorMessage = "Successfully accepted request";
            }
            return result;

        }
        public async Task<Response> CancelRequest()
        {
            throw new NotImplementedException();
        }

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<DataResponse<int>> CancelRequestOffer(RequestModel request, int userid)
        {
            var result = await _servicesDBUpdater.UpdateRequestDeny(request,userid).ConfigureAwait(false);
            if (result.Data == 1)
            {
                result.IsSuccessful = true;
                result.ErrorMessage = "Successfully updated request";
            }
            else
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Failed to cancel request";
            }
            return result;

            throw new NotImplementedException();
        }

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<DataResponse<int>> RequestService(RequestModel request)
        {
            var result = await _servicesDBInserter.InsertServiceReq(request).ConfigureAwait(false);
            if (result.Data == 0)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Failed to insert request";
            }
            else
            {
                result.IsSuccessful = true;
                result.ErrorMessage = "Successfully entered request";
            }
            return result;

        }

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<DataResponse<List<RequestModel>>> GetOfferRequests(int serv)
        {
            var result = await _servicesDBSelecter.GetProviderRequests(serv).ConfigureAwait(false);
            List<RequestModel> provrequests = new List<RequestModel>();
            List<ArrayList> requests = result.Data;
            DataResponse<List<RequestModel>> response = new DataResponse<List<RequestModel>>();
            foreach (var item in requests)
            {
                RequestModel requestModel = new RequestModel() { RequestID = (int)item[0], ServiceID = (int)item[1], ServiceName = (string)item[2], Requester = (int)item[3], RequestLat = (string)item[4], RequestLong = (string)item[5], PinType = (int)item[6], Accept = (int)item[7] };
                provrequests.Add(requestModel);
            }
            response.IsSuccessful = true;
            response.Data = provrequests;
            return response;
        }

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<DataResponse<List<RequestModel>>> GetUserRequests(int user)
        {
            
            var result = await _servicesDBSelecter.GetUserRequests(user).ConfigureAwait(false);
            List<RequestModel> provrequests = new List<RequestModel>();
            List<ArrayList> requests = result.Data;
            DataResponse<List<RequestModel>> response = new DataResponse<List<RequestModel>>();
            foreach (var item in requests)
            {
                RequestModel requestModel = new RequestModel() { RequestID = (int)item[0], ServiceID = (int)item[1], ServiceName = (string)item[2], Requester = (int)item[3], RequestLat = (string)item[4], RequestLong = (string)item[5], PinType = (int)item[6], Accept = (int)item[7] };
                if (user != requestModel.Requester)
                {
                    response.ErrorMessage = "Failed to get correct service requests";
                    response.IsSuccessful = false;
                    return response;
                }
                provrequests.Add(requestModel);
            }
            response.IsSuccessful = true;
            response.Data = provrequests;
            return response;
        }

        // TODO: Change to DataResponse with the the datatype you want to return back
        public async Task<DataResponse<List<ServiceModel>>> getservice(RequestModel request)
        {
            
            var result = await _servicesDBSelecter.getnearbyservice(request).ConfigureAwait(false);
            List<ServiceModel> servicelist = new List<ServiceModel>();
            List<ArrayList> services = result.Data;
            DataResponse<List<ServiceModel>> response = new DataResponse<List<ServiceModel>>();
            foreach (var item in services)
            {
                ServiceModel servModel = new ServiceModel() { ServiceName = (string)item[0], ServiceDescription = (string)item[1], ServicePhone = (string)item[2], ServiceURL = (string)item[3], ServiceID = (int)item[4], ServiceLat = (string)item[5], ServiceLong = (string)item[6], PinTypes = (int)item[7], Distance = (int)item[8] };
                servicelist.Add(servModel);
            }
            response.IsSuccessful = true;
            response.Data = servicelist;
            return response;
        }
    }
}