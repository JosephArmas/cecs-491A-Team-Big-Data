using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.ServiceOfferingsManagers.Interfaces;
using TeamBigData.Utification.ServiceOfferingsServices;

namespace TeamBigData.Utification.ServiceOfferingsManagers
{
    public class ServiceOfferingManager : IServiceOfferingManager
    {
        private readonly ServiceOfferingService _servService;
        private readonly ILogger _logger;
        public ServiceOfferingManager(ServiceOfferingService servService, ILogger logger)
        {
            _servService = servService;
            _logger = logger;
        }
        DataResponse<int> managerresult = new DataResponse<int>();
        /// <summary>
        /// Validates all of the required textual inputs for the ServModel
        /// </summary>
        /// <param name="server">The Service that needs to be validated</param>
        /// <returns>A Response. True being passing all test, false passing one test</returns>
        private DataResponse<int> ValidateServiceText(ServiceModel server)
        {

            Regex listofAcceptable = new Regex(@"^[\s\da-zA-Z.@áéíóúüñ¿¡ÁÉÍÓÚÜÑ/-]*$");
            Regex spaces = new Regex(@"[ ]");
            Regex phone = new Regex(@"^[\+]?(1)[(]?(209|213|310|323|408|415|424|442|510|530|559|562|619|626|650|657|661|669
                                    |707|714|747|760|805|818|831|858|909|916|925|949|951)[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4}$");
            MatchCollection titlematches = listofAcceptable.Matches(server.ServiceName);
            MatchCollection descmatches = listofAcceptable.Matches(server.ServiceDescription);
            var response = new DataResponse<int>();
            response.IsSuccessful = true;

            if (server.ServiceName.Length > 30 || server.ServiceName.Length < 4)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "title length";
            }

            if (titlematches.Count != 1)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "title char";
            }

            if (server.ServiceDescription.Length > 1000 || spaces.Matches(server.ServiceDescription).Count > 150)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "desc length";
            }

            if (descmatches.Count != 1)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "desc char";
            }


            if (phone.Matches(server.ServicePhone).Count != 1)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "phone area or country code";
            }
            if (server.ServicePhone.Length > 20)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "phone length";
            }

            return response;
        }

        private DataResponse<int> ValidatePinTypes(string pinStr)
        {
            var response = new DataResponse<int>();
            string validPins = @"^(?!.*(.).*\1)[0-4]+$";
            Regex pinLimit = new Regex(validPins);

            MatchCollection pins = pinLimit.Matches(pinStr);

            response.IsSuccessful = true;

            if (pins.Count != 1)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Invalid Pin Types";
            }

            return response;
        }


        /// <summary>
        /// The manager unregister method checks to make sure it follows business requirements and calls service layer
        /// </summary>
        /// <param name="serv">The service that is going to be unregistered</param>
        /// <returns>Response from the service layer</returns>
        public async Task<DataResponse<int>> unregister(ServiceModel serv)
        {



            if (serv == null)
            {
                managerresult.IsSuccessful = false;
                managerresult.ErrorMessage = "Manager ServiceModel parameter is null";
                return managerresult;
            }

            try
            {
                managerresult = await _servService.unregister(serv).ConfigureAwait(false);

            }

            catch (ArgumentException e)
            {

            }
            Log log;
            log = new Log(1, "Error", "User", "ServiceProviderManager.unregister()", "Data", "Error Select Pint Table returns empty.");
            await _logger.Logs(log).ConfigureAwait(false);
            return managerresult;
        }


        /// <summary>
        /// The manager user service creation method 
        /// </summary>
        /// <param name="serv">The service that is going to be registered</param>
        /// <returns>A response either from the service layer or the valid service text</returns>
        public async Task<DataResponse<int>> CreateService(ServiceModel serv)
        {
            if (serv == null)
            {
                managerresult.IsSuccessful = false;
                managerresult.ErrorMessage = "Manager ServiceModel parameter is null";
                return managerresult;
            }

            var pins = serv.PinTypes.ToString();
            var validText = ValidateServiceText(serv);
            var validPinTypes = ValidatePinTypes(pins);

            if (serv.ServiceURL.IsNullOrEmpty())
            {
                serv.ServiceURL = "N/A";
            }

            if (validPinTypes.IsSuccessful == false)
            {
                return validPinTypes;
            }

            if (validText.IsSuccessful == false)
            {
                return validText;
            }

            managerresult = await _servService.CreateServiceProv(serv).ConfigureAwait(false);

            return managerresult;
        }


        /// <summary>
        /// The manager validates inputs, logs, and calls the service layer
        /// </summary>
        /// <param name="serv">The Service being updated</param>
        /// <returns>Response of success or failure</returns>
        public async Task<DataResponse<int>> UpdateService(ServiceModel serv)
        {
            if (serv == null)
            {
                managerresult.IsSuccessful = false;
                managerresult.ErrorMessage = "Manager ServiceModel parameter is null";
                return managerresult;
            }

            var pins = serv.PinTypes.ToString();
            var validText = ValidateServiceText(serv);
            var validPinTypes = ValidatePinTypes(pins);

            if (serv.ServiceURL.IsNullOrEmpty())
            {
                serv.ServiceURL = "N/A";
            }

            if (validPinTypes.IsSuccessful == false)
            {
                return validPinTypes;
            }

            if (validText.IsSuccessful == false)
            {
                return validText;
            }

            managerresult = await _servService.UpdateServiceProv(serv).ConfigureAwait(false);

            return managerresult;
        }
    }
}
