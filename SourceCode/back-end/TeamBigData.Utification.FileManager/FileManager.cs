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
        public async Task<Response> UploadPinPic(String filename, int pinID, UserProfile cred)
        {
            var result = new Response();
            var ext = filename.Substring(filename.Length - 4, 4).ToLower();
            if (!(ext.Equals(".jpg") || ext.Equals(".png")))
            {
                result.errorMessage = "Unsupported File Extension";
                return result;
            }

            if(cred.Identity.AuthenticationType.Equals("Admin User"))
            {
                result = await service.UploadPinPic(filename, pinID);
            }
            else if(cred.Identity.AuthenticationType.Equals("Regular User") || cred.Identity.AuthenticationType.Equals("Reputable User"))
            {
                var getResponse = await _pinOwnerGetter.GetPinOwner(pinID);
                if(!getResponse.isSuccessful)
                {
                    result = getResponse;
                }
                else if(cred._userID != (int)getResponse.data)
                {
                    result.errorMessage = "Posting Issue. Invalid Action Performed";
                }
                else
                {
                    result = await service.UploadPinPic(filename, pinID);
                }
            }
            return result;
        }

        public async Task<Response> UploadProfilePic(String filename, int userID, UserProfile cred)
        {
            var result = new Response();
            var ext = filename.Substring(filename.Length - 4, 4).ToLower();
            if (!(ext.Equals(".jpg") || ext.Equals(".png")))
            {
                result.errorMessage = "Unsupported File Extension";
                return result;
            }
            if (cred.Identity.AuthenticationType.Equals("Admin User"))
            {
                result = await service.UploadProfilePic(filename, userID);
            }
            else if (cred.Identity.AuthenticationType.Equals("Regular User") || cred.Identity.AuthenticationType.Equals("Reputable User"))
            {
                if(userID == cred._userID)
                {
                    result = await service.UploadProfilePic(filename, userID);
                }
                else
                {
                    result.errorMessage = "Posting Issue. Invalid Action Performed";
                }
            }
            return result;
        }

        public async Task<Response> DeletePinPic(int pinID, UserProfile cred)
        {
            var result = new Response();

            if (cred.Identity.AuthenticationType.Equals("Admin User"))
            {
                result = await service.DeletePinPic(pinID);
            }
            else if (cred.Identity.AuthenticationType.Equals("Regular User") || cred.Identity.AuthenticationType.Equals("Reputable User"))
            {
                var getResponse = await _pinOwnerGetter.GetPinOwner(pinID);
                if (!getResponse.isSuccessful)
                {
                    result = getResponse;
                }
                else if (cred._userID != (int)getResponse.data)
                {
                    result.errorMessage = "Posting Issue. Invalid Action Performed";
                }
                else
                {
                    result = await service.DeletePinPic(pinID);
                }
            }
            return result;
        }

        public async Task<Response> DeleteProfilePic(int userID, UserProfile cred)
        {
            var result = new Response();

            if (cred.Identity.AuthenticationType.Equals("Admin User"))
            {
                result = await service.DeleteProfilePic(userID);
            }
            else if (cred.Identity.AuthenticationType.Equals("Regular User") || cred.Identity.AuthenticationType.Equals("Reputable User"))
            {
                if (userID == cred._userID)
                {
                    result = await service.DeleteProfilePic(userID);
                }
                else
                {
                    result.errorMessage = "Posting Issue. Invalid Action Performed";
                }
            }
            return result;
        }

        public async Task<Response> DownloadPinPic(int pinID)
        {
            var result = await _pinpicDownloader.DownloadPinPic(pinID);
            if (((String)result.data).Length > 0)
            {
                return result;
            }
            else
            {
                result.errorMessage = "Could not find uploaded file";
                return result;
            }
        }

        public async Task<Response> DownloadProfilePic(int userID)
        {
            var result = await _profilepicDownloader.DownloadProfilePic(userID);
            if (((String)result.data).Length > 0)
            {
                return result;
            }
            else
            {
                result.errorMessage = "Could not find uploaded file. Try Uploading a Profile Picture if you haven't yet";
                return result;
            }
        }

        public async Task<Response> UpdatePinPic(String filename, int pinID, UserProfile cred)
        {
            var result = new Response();
            var ext = filename.Substring(filename.Length - 4, 4).ToLower();
            if (!(ext.Equals(".jpg") || ext.Equals(".png")))
            {
                result.errorMessage = "Unsupported File Extension";
                return result;
            }
            if (cred.Identity.AuthenticationType.Equals("Admin User"))
            {
                result = await DownloadPinPic(pinID);
            }
            else if (cred.Identity.AuthenticationType.Equals("Regular User") || cred.Identity.AuthenticationType.Equals("Reputable User"))
            {
                var getResponse = await _pinOwnerGetter.GetPinOwner(pinID);
                if (!getResponse.isSuccessful)
                {
                    result = getResponse;
                }
                else if (cred._userID != (int)getResponse.data)
                {
                    result.errorMessage = "Posting Issue. Invalid Action Performed";
                }
                else
                {
                    result = await DownloadPinPic(pinID);
                }
            }
            return result;
        }

        public async Task<Response> UpdateProfilePic(String filename, int userID, UserProfile cred)
        {
            var result = new Response();
            var ext = filename.Substring(filename.Length - 4, 4).ToLower();
            if (!(ext.Equals(".jpg") || ext.Equals(".png")))
            {
                result.errorMessage = "Unsupported File Extension";
                return result;
            }
            if (cred.Identity.AuthenticationType.Equals("Admin User"))
            {
                result = await DownloadPinPic(userID);
            }
            else if (cred.Identity.AuthenticationType.Equals("Regular User") || cred.Identity.AuthenticationType.Equals("Reputable User"))
            {
                if(cred._userID == userID)
                {
                    result = await DownloadProfilePic(userID);
                }
                else
                {
                    result.errorMessage = "Posting Issue. Invalid Action Performed";
                }
            }
            return result;
        }
    }
}