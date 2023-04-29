using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Files;

namespace TeamBigData.Utification.FileServices
{
    public class FileService
    {
        private readonly IDBUploadPinPic _pinpicUploader;
        private readonly IDBUploadProfilePic _profilepicUploader;
        private readonly IDBDeletePinPic _pinpicDeleter;
        private readonly IDBDeleteProfilePic _profilepicDeleter;
        private readonly IDBDownloadPinPic _pinpicDownloader;
        private readonly IDBDownloadProfilePic _profilepicDownloader;

        public FileService(FileSqlDAO sqlDAO)
        {
            _pinpicUploader = sqlDAO;
            _profilepicUploader = sqlDAO;
            _pinpicDeleter = sqlDAO;
            _profilepicDeleter = sqlDAO;
            _pinpicDownloader = sqlDAO;
            _profilepicDownloader = sqlDAO;
        }

        public async Task<Response> UploadPinPic(String filename, int pinId)
        {
            var key = SecureHasher.Base64Hash(pinId + filename + pinId);
            var result = await _pinpicUploader.UploadPinPic(key, pinId);
            if(result.isSuccessful)
            {
                result.data = key;
            }
            if(result.errorMessage.Contains("PRIMARY KEY"))
            {
                result.errorMessage = "Can't Upload Picture Because Pin already has Picture, Use Update Instead";
            }
            return result;
        }
        public async Task<Response> UploadProfilePic(String filename, int userId)
        {
            var key = SecureHasher.Base64Hash(userId + filename + userId);
            var result = await _profilepicUploader.UploadProfilePic(key, userId);
            if (result.isSuccessful)
            {
                result.data = key;
            }
            if (result.errorMessage.Contains("PRIMARY KEY"))
            {
                result.errorMessage = "Can't Upload Picture Because Profile already has Picture, Use Update Instead";
            }
            return result;
        }
        public async Task<Response> DeletePinPic(int pinId)
        {
            var result = new Response();
            var keyResult = await _pinpicDownloader.DownloadPinPic(pinId);
            if (keyResult.isSuccessful)
            {
                result = await _pinpicDeleter.DeletePinPic(pinId);
                result.data = keyResult.data;
            }
            else
            {
                result.errorMessage = "Error Finding Picture to Delete";
            }
            return result;
        }
        public async Task<Response> DeleteProfilePic(int userId)
        {
            var result = new Response();
            var keyResult = await _profilepicDownloader.DownloadProfilePic(userId);
            if(keyResult.isSuccessful)
            {
                result = await _profilepicDeleter.DeleteProfilePic(userId);
                result.data = keyResult.data;
            }
            else
            {
                result.errorMessage = "Error Finding Picture to Delete";
            }
            return result;
        }
    }
}