
using System.Reflection;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.UserServices;

namespace TeamBigData.Utification.UserServicesManager
{
    public class ServRequestManager
    {
        protected readonly string logSql = @"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True";
        public async Task<Response> ProvAcceptRequest(RequestModel request)
        {
            ServReqService requestservice = new ServReqService();
            var requestresponse = new Response();

            requestresponse = await requestservice.ProvAcceptRequest(request).ConfigureAwait(false);


            Log log;
            var logger = new Logger(new SqlDAO(logSql));
            log = new Log(1, "Info", "User", MethodBase.GetCurrentMethod().Name, "Data", requestresponse.errorMessage);
            await logger.Log(log).ConfigureAwait(false);


            return requestresponse;
        }


        public async Task<Response> CancelRequest(RequestModel request)
        {
            ServReqService requestservice = new ServReqService();
            var requestresponse = new Response();

            requestresponse = await requestservice.CancelRequest().ConfigureAwait(false);

            Log log;
            var logger = new Logger(new SqlDAO(logSql));
            log = new Log(1, "Info", "User", MethodBase.GetCurrentMethod().Name, "Data", requestresponse.errorMessage);
            await logger.Log(log).ConfigureAwait(false);

            return requestresponse;

        }


        public async Task<Response> ProvCancelRequest(RequestModel request)
        {
            ServReqService requestservice = new ServReqService();
            var requestresponse = new Response();

            requestresponse = await requestservice.ProvCancelRequest(request).ConfigureAwait(false);

            Log log;
            var logger = new Logger(new SqlDAO(logSql));
            log = new Log(1, "Info", "User", MethodBase.GetCurrentMethod().Name, "Data", requestresponse.errorMessage);
            await logger.Log(log).ConfigureAwait(false);

            return requestresponse;

        }


        public async Task<Response> getservice(Pin pin,int dist)
        {
            ServReqService requestservice = new ServReqService();
            var requestresponse = new Response();

            requestresponse = await requestservice.getservice(pin,dist).ConfigureAwait(false);

            Log log;
            var logger = new Logger(new SqlDAO(logSql));
            log = new Log(1, "Info", "User", MethodBase.GetCurrentMethod().Name, "Data", requestresponse.errorMessage);
            await logger.Log(log).ConfigureAwait(false);

            return requestresponse;
        }


        public async Task<Response> ServiceRequest(ServModel company,Pin pin)
        {
            ServReqService requestservice = new ServReqService();
            var requestresponse = new Response();

            requestresponse = await requestservice.ServiceRequest(company,pin).ConfigureAwait(false);

            Log log;
            var logger = new Logger(new SqlDAO(logSql));
            log = new Log(1, "Info", "User", MethodBase.GetCurrentMethod().Name, "Data", requestresponse.errorMessage);
            await logger.Log(log).ConfigureAwait(false);

            return requestresponse;
        }


        public async Task<Response> GetProviderRequests(ServModel serv)
        {
            ServReqService requestservice = new ServReqService();
            var requestresponse = new Response();

            requestresponse = await requestservice.GetProviderRequests(serv).ConfigureAwait(false);

            Log log;
            var logger = new Logger(new SqlDAO(logSql));
            log = new Log(1, "Info", "User", MethodBase.GetCurrentMethod().Name, "Data", requestresponse.errorMessage);
            await logger.Log(log).ConfigureAwait(false);

            return requestresponse;
        }


        public async Task<Response> GetUserRequests(UserProfile user)
        {
            ServReqService requestservice = new ServReqService();
            var requestresponse = new Response();

            requestresponse = await requestservice.GetUserRequests(user).ConfigureAwait(false);

            Log log;
            var logger = new Logger(new SqlDAO(logSql));
            log = new Log(1, "Info", "User", MethodBase.GetCurrentMethod().Name, "Data", requestresponse.errorMessage);
            await logger.Log(log).ConfigureAwait(false);

            return requestresponse;
        }
    }
}