/*using Microsoft.AspNetCore.Mvc;
using TeamBigData.Utification.FileManagers;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.ErrorResponse;

namespace Utification.EntryPoint.Controllers
{
    public class FileInput
    {
        public String fileName { get; set; }
        public int ID { get; set; }

        public String role { get; set; }

        public int userID { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        [Route("pinUpload")]
        [HttpPost]
        public async Task<IActionResult> PinUpload([FromBody] FileInput input)
        {
            var manager = new FileManager();
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
            var manager = new FileManager();
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
            var manager = new FileManager();
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
            var manager = new FileManager();
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
            var manager = new FileManager();
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
            var manager = new FileManager();
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
            var manager = new FileManager();
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
            var manager = new FileManager();
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
*/