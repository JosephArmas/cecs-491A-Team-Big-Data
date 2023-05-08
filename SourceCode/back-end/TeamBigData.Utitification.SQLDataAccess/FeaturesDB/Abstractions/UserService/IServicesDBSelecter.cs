using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.UserServices
{
    public interface IServicesDBSelecter
    {
        public Task<DataResponse<List<ArrayList>>> getnearbyservice(RequestModel request);
        public Task<DataResponse<int>> GetServiceCount();

        public Task<DataResponse<List<ArrayList>>> GetProviderRequests(int serv);

        public Task<DataResponse<List<ArrayList>>> GetUserRequests(int user);


    }
}
