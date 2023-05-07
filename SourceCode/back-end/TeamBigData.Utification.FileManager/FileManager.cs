using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.FileServices;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Files;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Pins;

namespace TeamBigData.Utification.FileManagers
{
    public class FileManager
    {
        private readonly FileService service;
        private readonly IDBDownloadPinPic _pinpicDownloader;
        private readonly IDBDownloadProfilePic _profilepicDownloader;
        private readonly IDBSelectPinOwner _pinOwnerGetter;
        public FileManager(FileService fileService, FileSqlDAO sqlDAO, PinsSqlDAO pinDAO)
        {
            service = fileService;
            _pinpicDownloader = sqlDAO;
            _profilepicDownloader = sqlDAO;
            _pinOwnerGetter = pinDAO;
        }
        public async Task<DataResponse<String>> UploadPinPic(String filename, int pinID, UserProfile cred)
        {
            var dataResult = new DataResponse<String>();
            var ext = filename.Substring(filename.Length - 4, 4).ToLower();
            if (!(ext.Equals(".jpg") || ext.Equals(".png")))
            {
                dataResult.ErrorMessage = "Unsupported File Extension";
                return dataResult;
            }

            if(cred.Identity.AuthenticationType.Equals("Admin User"))
            {
                dataResult = await service.UploadPinPic(filename, pinID);
            }
            else if(cred.Identity.AuthenticationType.Equals("Regular User") || cred.Identity.AuthenticationType.Equals("Reputable User"))
            {
                var getResponse = await _pinOwnerGetter.GetPinOwner(pinID);
                if(!getResponse.IsSuccessful)
                {
                    dataResult = new DataResponse<String>(getResponse.IsSuccessful, getResponse.ErrorMessage, "");
                }
                else if(cred.UserID != (int)getResponse.Data)
                {
                    dataResult.IsSuccessful = false;
                    dataResult.ErrorMessage = "Posting Issue. Invalid Action Performed";
                }
                else
                {
                    dataResult = await service.UploadPinPic(filename, pinID);
                }
            }
            return dataResult;
        }

        public async Task<DataResponse<String>> UploadProfilePic(String filename, int userID, UserProfile cred)
        {
            var result = new Response();
            var dataResult = new DataResponse<String>();
            var ext = filename.Substring(filename.Length - 4, 4).ToLower();
            if (!(ext.Equals(".jpg") || ext.Equals(".png")))
            {
                dataResult.ErrorMessage = "Unsupported File Extension";
                return dataResult;
            }
            if (cred.Identity.AuthenticationType.Equals("Admin User"))
            {
                dataResult = await service.UploadProfilePic(filename, userID);
            }
            else if (cred.Identity.AuthenticationType.Equals("Regular User") || cred.Identity.AuthenticationType.Equals("Reputable User"))
            {
                if(userID == cred.UserID)
                {
                    dataResult = await service.UploadProfilePic(filename, userID);
                }
                else
                {
                    dataResult.IsSuccessful = false;
                    dataResult.ErrorMessage = "Posting Issue. Invalid Action Performed";
                }
            }
            return dataResult;
        }

        public async Task<DataResponse<String>> DeletePinPic(int pinID, UserProfile cred)
        {
            var result = new Response();
            var dataResult = new DataResponse<String>();

            if (cred.Identity.AuthenticationType.Equals("Admin User"))
            {
                dataResult = await service.DeletePinPic(pinID);
            }
            else if (cred.Identity.AuthenticationType.Equals("Regular User") || cred.Identity.AuthenticationType.Equals("Reputable User"))
            {
                var getResponse = await _pinOwnerGetter.GetPinOwner(pinID);
                if (!getResponse.IsSuccessful)
                {
                    dataResult.IsSuccessful = getResponse.IsSuccessful;
                    dataResult.ErrorMessage = getResponse.ErrorMessage;
                    return dataResult;
                }
                else if (cred.UserID != (int)getResponse.Data)
                {
                    dataResult.ErrorMessage = "Posting Issue. Invalid Action Performed";
                }
                else
                {
                    dataResult = await service.DeletePinPic(pinID);
                }
            }
            return dataResult;
        }

        public async Task<DataResponse<String>> DeleteProfilePic(int userID, UserProfile cred)
        {
            var result = new DataResponse<String>();

            if (cred.Identity.AuthenticationType.Equals("Admin User"))
            {
                result = await service.DeleteProfilePic(userID);
            }
            else if (cred.Identity.AuthenticationType.Equals("Regular User") || cred.Identity.AuthenticationType.Equals("Reputable User"))
            {
                if (userID == cred.UserID)
                {
                    result = await service.DeleteProfilePic(userID);
                }
                else
                {
                    result.ErrorMessage = "Posting Issue. Invalid Action Performed";
                }
            }
            return result;
        }

        public async Task<DataResponse<String>> DownloadPinPic(int pinID)
        {
            var result = await _pinpicDownloader.DownloadPinPic(pinID);
            if (result.Data.Length > 0)
            {
                return result;
            }
            else
            {
                result.ErrorMessage = "Could not find uploaded file";
                return result;
            }
        }

        public async Task<DataResponse<String>> DownloadProfilePic(int userID)
        {
            var result = await _profilepicDownloader.DownloadProfilePic(userID);
            if (result.Data.Length > 0)
            {
                result.IsSuccessful = true;
                return result;
            }
            else
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Could not find uploaded file. Try Uploading a Profile Picture if you haven't yet";
                return result;
            }
        }

        public async Task<DataResponse<String>> UpdatePinPic(String filename, int pinID, UserProfile cred)
        {
            var result = new DataResponse<String>();
            var ext = filename.Substring(filename.Length - 4, 4).ToLower();
            if (!(ext.Equals(".jpg") || ext.Equals(".png")))
            {
                result.ErrorMessage = "Unsupported File Extension";
                return result;
            }
            if (cred.Identity.AuthenticationType.Equals("Admin User"))
            {
                result = await DownloadPinPic(pinID);
            }
            else if (cred.Identity.AuthenticationType.Equals("Regular User") || cred.Identity.AuthenticationType.Equals("Reputable User"))
            {
                var getResponse = await _pinOwnerGetter.GetPinOwner(pinID);
                if (!getResponse.IsSuccessful)
                {
                    return result;
                }
                else if (cred.UserID != (int)getResponse.Data)
                {
                    result.ErrorMessage = "Posting Issue. Invalid Action Performed";
                }
                else
                {
                    result = await DownloadPinPic(pinID);
                }
            }
            return result;
        }

        public async Task<DataResponse<String>> UpdateProfilePic(String filename, int userID, UserProfile cred)
        {
            var result = new DataResponse<String>();
            var ext = filename.Substring(filename.Length - 4, 4).ToLower();
            if (!(ext.Equals(".jpg") || ext.Equals(".png")))
            {
                result.ErrorMessage = "Unsupported File Extension";
                return result;
            }
            if (cred.Identity.AuthenticationType.Equals("Admin User"))
            {
                result = await DownloadPinPic(userID);
            }
            else if (cred.Identity.AuthenticationType.Equals("Regular User") || cred.Identity.AuthenticationType.Equals("Reputable User"))
            {
                if(cred.UserID == userID)
                {
                    result = await DownloadProfilePic(userID);
                }
                else
                {
                    result.ErrorMessage = "Posting Issue. Invalid Action Performed";
                }
            }
            return result;
        }
    }
}