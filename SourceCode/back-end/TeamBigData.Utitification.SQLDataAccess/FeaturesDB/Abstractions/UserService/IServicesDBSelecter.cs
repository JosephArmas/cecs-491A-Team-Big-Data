using System;
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
        public Task<Response> getnearbyservice(Pin pin, int dist);
        public Task<Response> GetServiceCount();

        public Task<Response> GetProviderRequests(ServiceModel serv);

        public Task<Response> GetUserRequests(UserProfile user);


    }
}
