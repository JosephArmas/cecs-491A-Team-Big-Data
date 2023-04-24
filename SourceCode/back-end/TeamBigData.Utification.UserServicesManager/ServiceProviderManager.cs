using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.UserServices;
using TeamBigData.Utification.UserServicesManager.Interfaces;

namespace TeamBigData.Utification.UserServicesManager
{
    public class ServiceProviderManager : IServiceProviderManager
    {

        protected readonly string logSql = @"Server=.\;Database=TeamBigData.Utification.Logs;User=AppUser;Password=t;TrustServerCertificate=True;Encrypt=True";

        /// <summary>
        /// Validates all of the required textual inputs for the ServModel
        /// </summary>
        /// <param name="Server">The Service that needs to be validated</param>
        /// <returns>A Response. True being passing all test, false passing one test</returns>
        private Response ValidateServiceText(ServModel Server)
        {;
            Regex listofAcceptable = new Regex(@"^[\s\da-zA-Z.@áéíóúüñ¿¡ÁÉÍÓÚÜÑ/-]*$");

            Regex spaces = new Regex(@"[ ]");

            Regex phone = new Regex(@"^[\+]?(1)[(]?(209|213|310|323|408|415|424|442|510|530|559|562|619|626|650|657|661|669
                                    |707|714|747|760|805|818|831|858|909|916|925|949|951)[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4}$");

            MatchCollection titlematches = listofAcceptable.Matches(Server.ServiceName);
            MatchCollection descmatches = listofAcceptable.Matches(Server.ServiceDescription);

            var response = new Response();
            response.isSuccessful = true;

            if (Server.ServiceName.Length > 30 || Server.ServiceName.Length < 4)
            {
                response.isSuccessful = false;
                response.errorMessage = "Syntax Error. Title is over 30 Character Limit";
                response.data = Server.ServiceName;
            }

            if(titlematches.Count != 1)
            {
                response.isSuccessful = false;
                response.errorMessage = "title char";
                response.data = Server.ServiceName;
            }

            if(Server.ServiceDescription.Length > 1000 || spaces.Matches(Server.ServiceDescription).Count > 150)
            {
                response.isSuccessful = false;
                response.errorMessage = "Syntax Error. Description is over 150 Word Limit";
                response.data = Server.ServiceDescription;
            }
            
            if (descmatches.Count != 1)
            {
                response.isSuccessful = false;
                response.errorMessage = "desc char";
                response.data = Server.ServiceDescription;
            }


            if (phone.Matches(Server.ServicePhone).Count != 1)
            {
                response.isSuccessful = false;
                response.errorMessage = "phone area or country code";
                response.data = Server.ServicePhone;
            }

            if (Server.ServicePhone.Length > 20)
            {
                response.isSuccessful = false;
                response.errorMessage = "phone length";
                response.data = Server.ServicePhone;
            }

            return response;
        }

        private Response ValidatePinTypes(string pinStr)
        {
            var response = new Response();

            string validPins = @"^(?!.*(.).*\1)[1-5]+$";
            Regex pinLimit = new Regex(validPins);
            MatchCollection pins = pinLimit.Matches(pinStr);

            if (pins.Count != 1)
            {
                response.isSuccessful = false;
                response.errorMessage = "Invalid Pin Types";
                response.data = pinStr;
            }
            else
            {
                response.isSuccessful = true;
            }
            return response;
        }


        /// <summary>
        /// The manager unregister method checks to make sure it follows business requirements and calls service layer
        /// </summary>
        /// <param name="serv">The service that is going to be unregistered</param>
        /// <returns>Response from the service layer</returns>
        public async Task<Response> unregister(ServModel serv)
        {
            ServProviderService ProviderService = new ServProviderService();

            var result = new Response();
            try 
            { 
                result = await ProviderService.unregister(serv).ConfigureAwait(false);
                
            }
            catch (ArgumentException e)
            {
                Log log;
                var logger = new Logger(new SqlDAO(logSql));
                log = new Log(1, "Error", "User", "ServiceProviderManager.unregister()", "Data", e.Message);
                await logger.Log(log).ConfigureAwait(false);
            }


            return result;
        }


        /// <summary>
        /// The manager user service creation method 
        /// </summary>
        /// <param name="serv">The service that is going to be registered</param>
        /// <returns>A response either from the service layer or the valid service text</returns>
        public async Task<Response> CreateService(ServModel serv)
        {
            ServProviderService ProviderService = new ServProviderService();
            Response result = new Response();

            var pins = serv.PinTypes.ToString();
            var validText = ValidateServiceText(serv);
            var validPinTypes = ValidatePinTypes(pins);

            if(serv.ServiceURL == null || serv.ServiceURL == "")
            {
                serv.ServiceURL = "N/A";
            }
            
            if (validText.isSuccessful && validPinTypes.isSuccessful)
            {
                try
                {
                    result = await ProviderService.CreateServiceProv(serv).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Log log;
                    var logger = new Logger(new SqlDAO(logSql));
                    log = new Log(1, "Error", "User", MethodBase.GetCurrentMethod().Name, "Data", e.Message);
                    await logger.Log(log).ConfigureAwait(false);
                }
                return result;
            }

            else if (validPinTypes.isSuccessful == false)
            {
                return validPinTypes;
            }

            else
            {
                return validText;
            }
        }


        /// <summary>
        /// The manager validates inputs, logs, and calls the service layer
        /// </summary>
        /// <param name="serv">The Service being updated</param>
        /// <returns>Response of success or failure</returns>
        public async Task<Response> UpdateService(ServModel serv)
        {
            ServProviderService ProviderService = new ServProviderService();
            Response result = new Response();

            var pins = serv.PinTypes.ToString();
            var validText = ValidateServiceText(serv);
            var validPinTypes = ValidatePinTypes(pins);

            if (validText.isSuccessful)
            {
                try
                {
                    result = await ProviderService.UpdateServiceProv(serv).ConfigureAwait(false);
                }
                catch(Exception e)
                {
                    Log log;
                    var logger = new Logger(new SqlDAO(logSql));
                    log = new Log(1, "Error", "User", MethodBase.GetCurrentMethod().Name, "Data", e.Message);
                    await logger.Log(log).ConfigureAwait(false);
                }
                return result;
            }
            else if (validPinTypes.isSuccessful == false)
            {
                return validPinTypes;
            }
            else
            {
                return validText;
            }
        }
    }
}
