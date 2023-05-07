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

        public async Task<DataResponse<String>> UploadPinPic(String filename, int pinId)
        {
            var key = SecureHasher.Base64Hash(pinId + filename + pinId);
            var result = await _pinpicUploader.UploadPinPic(key, pinId);
            if(result.IsSuccessful)
            {
                return new DataResponse<String>(result.IsSuccessful, result.ErrorMessage, key);
            }
            if(result.ErrorMessage.Contains("PRIMARY KEY"))
            {
                result.ErrorMessage = "Can't Upload Picture Because Pin already has Picture, Use Update Instead";
            }
            return new DataResponse<String>(result.IsSuccessful, result.ErrorMessage, "");
        }
        public async Task<DataResponse<String>> UploadProfilePic(String filename, int userId)
        {
            var key = SecureHasher.Base64Hash(userId + filename + userId);
            var result = await _profilepicUploader.UploadProfilePic(key, userId);
            if (result.IsSuccessful)
            {
                return new DataResponse<String>(result.IsSuccessful, result.ErrorMessage, key);
            }
            if (result.ErrorMessage.Contains("PRIMARY KEY"))
            {
                result.ErrorMessage = "Can't Upload Picture Because Profile already has Picture, Use Update Instead";
            }
            return new DataResponse<String>(result.IsSuccessful, result.ErrorMessage, "");
        }
        public async Task<DataResponse<String>> DeletePinPic(int pinId)
        {
            var result = new Response();
            var keyResult = await _pinpicDownloader.DownloadPinPic(pinId);
            if (!keyResult.IsSuccessful)
            {
                keyResult.IsSuccessful = false;
                keyResult.ErrorMessage = "Error Finding Picture to Delete";
            }
            else
            {
                result = await _pinpicDeleter.DeletePinPic(pinId);
                keyResult.IsSuccessful = true;
            }
            return keyResult;
        }
        public async Task<DataResponse<String>> DeleteProfilePic(int userId)
        {
            var result = new Response();
            var keyResult = await _profilepicDownloader.DownloadProfilePic(userId);
            if (!keyResult.IsSuccessful)
            {
                keyResult.IsSuccessful = false;
                keyResult.ErrorMessage = "Error Finding Picture to Delete";
            }
            else
            {
                result = await _profilepicDeleter.DeleteProfilePic(userId);
                keyResult.IsSuccessful = true;
            }
            return keyResult;
        }
    }
}