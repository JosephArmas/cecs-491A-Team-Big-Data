using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.FileManagers;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.Models.ControllerModels;
using TeamBigData.Utification.ErrorResponse;

namespace Utification.EntryPoint.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly FileManager manager;
        public FileController(FileManager fileManager)
        {
            manager = fileManager;
        }

        [Route("pinUpload")]
        [HttpPost]
        public async Task<IActionResult> PinUpload([FromBody] FileInput input)
        {
            var profile = new UserProfile(input.userID, input.role);
            var result = new Response();
            try
            {
                result = await manager.UploadPinPic(input.fileName, input.ID, profile);
            }
            catch
            {

            }
            if(result.isSuccessful)
            {
                return Ok((String)result.data);
            }
            else
            {
                return Conflict(result.errorMessage);
            }
        }

        [Route("pinDownload")]
        [HttpPost]
        public async Task<IActionResult> PinDownload([FromHeader] int ID)
        {
            var result = new Response();
            try
            {
                result = await manager.DownloadPinPic(ID);
            }
            catch
            {

            }
            if (result.isSuccessful)
            {
                return Ok((String)result.data);
            }
            else
            {
                return Conflict(result.errorMessage);
            }
        }

        [Route("pinUpdate")]
        [HttpPost]
        public async Task<IActionResult> PinUpdate([FromBody] FileInput input)
        {
            var profile = new UserProfile(input.userID, input.role);
            var result = new Response();
            try
            {
                result = await manager.UpdatePinPic(input.fileName, input.ID, profile);
            }
            catch
            {

            }
            if (result.isSuccessful)
            {
                return Ok((String)result.data);
            }
            else
            {
                return Conflict(result.errorMessage);
            }
        }

        [Route("pinDelete")]
        [HttpPost]
        public async Task<IActionResult> PinDelete([FromBody] FileInput input)
        {
            var profile = new UserProfile(input.userID, input.role);
            var result = new Response();
            try
            {
                result = await manager.DeletePinPic(input.ID, profile);
            }
            catch
            {

            }
            if (result.isSuccessful)
            {
                return Ok((String)result.data);
            }
            else
            {
                return Conflict(result.errorMessage);
            }
        }

        [Route("profileUpload")]
        [HttpPost]
        public async Task<IActionResult> ProfileUpload([FromBody] FileInput input)
        {
            var profile = new UserProfile(input.userID, input.role);
            var result = new Response();
            try
            {
                result = await manager.UploadProfilePic(input.fileName, input.ID, profile);
            }
            catch
            {

            }
            if (result.isSuccessful)
            {
                return Ok((String)result.data);
            }
            else
            {
                return Conflict(result.errorMessage);
            }
        }

        [Route("profileDownload")]
        [HttpPost]
        public async Task<IActionResult> ProfileDownload([FromHeader] int ID)
        {
            var result = new Response();
            try
            {
                result = await manager.DownloadProfilePic(ID);
            }
            catch
            {

            }
            if (result.isSuccessful)
            {
                return Ok((String)result.data);
            }
            else
            {
                return Conflict(result.errorMessage);
            }
        }

        [Route("profileUpdate")]
        [HttpPost]
        public async Task<IActionResult> ProfileUpdate([FromBody] FileInput input)
        {
            var profile = new UserProfile(input.userID, input.role);
            var result = new Response();
            try
            {
                result = await manager.UpdateProfilePic(input.fileName, input.ID, profile);
            }
            catch
            {

            }
            if (result.isSuccessful)
            {
                return Ok((String)result.data);
            }
            else
            {
                return Conflict(result.errorMessage);
            }
        }

        [Route("profileDelete")]
        [HttpPost]
        public async Task<IActionResult> ProfileDelete([FromBody] FileInput input)
        {
            var profile = new UserProfile(input.userID, input.role);
            var result = new Response();
            try
            {
                result = await manager.DeleteProfilePic(input.ID, profile);
            }
            catch
            {

            }
            if (result.isSuccessful)
            {
                return Ok((String)result.data);
            }
            else
            {
                return Conflict(result.errorMessage);
            }
        }
    }
}
