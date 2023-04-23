using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.Cryptography;
namespace TeamBigData.Utification.FileServices
{
    public class FileService
    {
        public async Task<Response> UploadPinPic(String filename, int pinId, String connectionString)
        {
            var key = SecureHasher.Base64Hash(pinId + filename + pinId);
            var fileDao = new SqlDAO(connectionString);
            var result = await fileDao.UploadPinPic(key, pinId);
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
        public async Task<Response> UploadProfilePic(String filename, int userId, String connectionString)
        {
            var key = SecureHasher.Base64Hash(userId + filename + userId);
            var fileDao = new SqlDAO(connectionString);
            var result = await fileDao.UploadProfilePic(key, userId);
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
        public async Task<Response> DeletePinPic(int pinId, String connectionString)
        {
            var result = new Response();
            var fileDao = new SqlDAO(connectionString);
            var keyResult = await fileDao.DownloadPinPic(pinId);
            if (keyResult.isSuccessful)
            {
                result = await fileDao.DeletePinPic(pinId);
                result.data = keyResult.data;
            }
            else
            {
                result.errorMessage = "Error Finding Picture to Delete";
            }
            return result;
        }
        public async Task<Response> DeleteProfilePic(int userId, String connectionString)
        {
            var result = new Response();
            var fileDao = new SqlDAO(connectionString);
            var keyResult = await fileDao.DownloadProfilePic(userId);
            if(keyResult.isSuccessful)
            {
                result = await fileDao.DeleteProfilePic(userId);
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