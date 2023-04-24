using System.Collections;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;

namespace TeamBigData.Utification.UserServices
{
    public class ServReqService
    {
        private string _connectionString = @"Server=.\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False";
        public async Task<Response> ProvAcceptRequest(RequestModel request)
        {
            SqlDAO dao = new SqlDAO(_connectionString);
            var result = await dao.UpdateRequestAccept(request).ConfigureAwait(false);
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
            SqlDAO dao = new SqlDAO(_connectionString);
            throw new NotImplementedException();
        }



        public async Task<Response> ProvCancelRequest(RequestModel request)
        {
            SqlDAO dao = new SqlDAO(_connectionString);

            var result = await dao.UpdateRequestDeny(request).ConfigureAwait(false);

            if ((int)result.data == 1)
            {
                result.isSuccessful = true;
                result.errorMessage =  "Successfully updated request";
            }
            else
            {
                result.isSuccessful = false;
                result.errorMessage = "Failed to cancel request";
            }
            return result;
        }



        public async Task<Response> ServiceRequest(ServModel company, Pin pin) 
        {
            SqlDAO dao = new SqlDAO(_connectionString);

            var result = await dao.InsertServiceReq(company,pin).ConfigureAwait(false);

            if((int)result.data == 0)
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



        public async Task<Response> GetProviderRequests(ServModel serv)
        {
            SqlDAO dao = new SqlDAO(_connectionString);


            var result = await dao.GetProviderRequests(serv).ConfigureAwait(false);


            List < RequestModel > provrequests = new List<RequestModel>();

            List<ArrayList> requests = (List<ArrayList>)result.data;

            foreach (var item in requests)
            {
                RequestModel requestModel = new RequestModel() { RequestID = (int)item[0], ServiceID = (int)item[1], ServiceName = (string)item[2], Requester = (int)item[3], RequestLat = (string)item[4], RequestLong = (string)item[5], PinType = (int)item[6], Accept = (int)item[7] };
                
                if(requestModel.ServiceID != serv.ServiceID || requestModel.ServiceName !=  serv.ServiceName)
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
            SqlDAO dao = new SqlDAO(_connectionString);
            var result = await dao.GetUserRequests(user).ConfigureAwait(false);
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
            SqlDAO dao = new SqlDAO(_connectionString);


            var result = await dao.getnearbyservice(pin,dist).ConfigureAwait(false);


            List<ServModel> servicelist = new List<ServModel>();

            List<ArrayList> services = (List<ArrayList>)result.data;

            foreach (var item in services)
            {
                ServModel servModel = new ServModel() { ServiceName = (string)item[0], ServiceDescription = (string)item[1], ServicePhone = (string)item[2], ServiceURL = (string)item[3], ServiceID = (int)item[4], ServiceLat = (string)item[5], ServiceLong = (string)item[6], PinTypes = (int)item[7], Distance = (int)item[8] };
                servicelist.Add(servModel);
            }

            result.isSuccessful = true;

            result.data = servicelist;

            return result; 
        }
    }
}