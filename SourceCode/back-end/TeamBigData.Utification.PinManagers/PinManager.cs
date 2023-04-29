﻿using TeamBigData.Utification.Models;
using TeamBigData.Utification.PinServices;
using TeamBigData.Utification.ErrorResponse;
using System.Reflection.Metadata;
using System.Security.Claims;
using TeamBigData.Utification.SQLDataAccess.DTO;
using TeamBigData.Utification.Logging.Abstraction;

namespace TeamBigData.Utification.PinManagers
{
    public class PinManager
    {
        private readonly PinService _pinService;
        private readonly ILogger _logger;

        public PinManager(PinService pinService, ILogger logger) 
        { 
            _pinService = pinService;
            _logger = logger;
        }
        public async Task<Response> SaveNewPin(Pin pin, String userhash)
        {
            // TODO: Check if process time follows business rules

            await _logger.Logs(new Log(0, "Info", userhash, "Save New Pin Attempt", "Data", "User is attempting to save new pin."));

            var response = await _pinService.StoreNewPin(pin).ConfigureAwait(false);
            if (!response.isSuccessful)
            {
                await _logger.Logs(new Log(0, "Error", userhash, "_pinService.StoreNewPin", "Data", "Failed to store new pin."));

                response.isSuccessful = false;
                response.errorMessage += ", {false: _pinService.StoreNewPin}";
                return response;
            }
            else
            {
                await _logger.Logs(new Log(0, "Info", userhash, "Passed Save New Pin Attempt", "Data", "User successfully save new pin."));

                response.isSuccessful = true;
            }

            return response;
        }

        public async Task<DataResponse<List<PinResponse>>> GetListOfAllPins(String userhash)
        {
            // TODO: Check if process time follows business rules
            await _logger.Logs(new Log(0, "Info", userhash, "Get List Of All Pins Attempt", "Data", "User is attempting to get list of all pins."));

            var pinResponse = await _pinService.GetPinTable().ConfigureAwait(false);
            if (!pinResponse.isSuccessful)
            {
                await _logger.Logs(new Log(0, "Error", userhash, "_pinService.GetPinTable", "Data", "Failed to get list of new pin."));

                pinResponse.isSuccessful = false;
                pinResponse.errorMessage += ", {failed: _pinService.GetPinTable}";
                return pinResponse;
            }
            else
            {
                await _logger.Logs(new Log(0, "Info", userhash, "Passed Get List Of All Pins Attempt", "Data", "User Successfully get list of all pins."));

                pinResponse.isSuccessful = true;
            }

            return pinResponse;
        }

        public async Task<Response> MarkAsCompletedPin(int pinID, int userID, String userhash)
        {
            // TODO: Check if process time follows business rules
            await _logger.Logs(new Log(0, "Info", userhash, "Mark As Completed Pin Attempt", "Data", "User is attempting to mark as complete pin."));

            var response = await _pinService.MarkAsCompleted(pinID, userID).ConfigureAwait(false);
            if (!response.isSuccessful)
            {
                await _logger.Logs(new Log(0, "Error", userhash, "_pinService.MarkAsCompleted", "Data", "Failed to mark as completed."));

                response.isSuccessful = false;
                response.errorMessage += ", {failed: _pinService.MarkAsCompleted}";
                return response;
            }
            else
            {
                await _logger.Logs(new Log(0, "Info", userhash, "Passed Mark As Completed Pin Attempt", "Data", "User successfully mark as complete pin."));
                response.isSuccessful = true; 
            }

            return response;
        }

        public async Task<Response> ChangePinContent(int pinID, int userID, String description, String userhash)
        {
            // TODO: Check if process time follows business rules
            await _logger.Logs(new Log(0, "Info", userhash, "Change Pin Content Attempt", "Data", "User is attempting to change pin content."));

            var response = await _pinService.ChangePinContentTo(pinID, userID, description).ConfigureAwait(false);
            if (!response.isSuccessful)
            {
                await _logger.Logs(new Log(0, "Error", userhash, "_pinService.ChangePinContentTo", "Data", "Failed to change pin content."));

                response.isSuccessful = false;
                response.errorMessage += ", {failed: _pinService.ChangePinContentTo}";
            }
            else 
            {
                await _logger.Logs(new Log(0, "Info", userhash, "Passed Change Pin Content Attempt", "Data", "User successfully change pin content."));

                response.isSuccessful = true;
            }

            return response;
        }
        public async Task<Response> ChangePinType(int pinID, int userID, int pinType, String userhash)
        {
            // TODO: Check if process time follows business rules
            await _logger.Logs(new Log(0, "Info", userhash, "Change Pin Type Attempt", "Data", "User is attempting to change pin type."));

            var response = await _pinService.ChangePinTypeTo(pinID, userID, pinType).ConfigureAwait(false);
            if (!response.isSuccessful)
            {
                await _logger.Logs(new Log(0, "Error", userhash, "_pinService.ChangePinTypeTo", "Data", "Failed to change pin type."));

                response.isSuccessful = false;
                response.errorMessage += ", {failed: _pinService.ChangePinTypeTo}";
                return response;
            }
            else
            {
                await _logger.Logs(new Log(0, "Info", userhash, "Passed Change Pin Type Attempt", "Data", "User successfully change pin type."));

                response.isSuccessful = true; 
            }

            return response;
        }

        
        public async Task<Response> DisablePin(int pinID, int userID, string userhash)
        {
            // TODO: Check if process time follows business rules
            await _logger.Logs(new Log(0, "Info", userhash, "Disable Pin Attempt", "Data", "Admin is attempting to disable pin."));

            var response = await _pinService.DisablingPin(pinID, userID).ConfigureAwait(false);
            if (!response.isSuccessful)
            {
                await _logger.Logs(new Log(0, "Error", userhash, "_pinService.DisablingPin", "Data", "Failed to disable pin."));

                response.isSuccessful = false;
                response.errorMessage += ", {failed: _pinService.DisablingPin}";
                return response;
            }
            else
            {
                await _logger.Logs(new Log(0, "Info", userhash, "Passed Disable Pin Attempt", "Data", "Admin successfully disable pin."));

                response.isSuccessful = true; 
            }

            return response;
        }
    }
}