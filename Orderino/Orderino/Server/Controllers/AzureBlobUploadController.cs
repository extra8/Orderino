using Microsoft.AspNetCore.Mvc;
using Orderino.Infrastructure.AzureBlob.Interfaces;
using System.Threading.Tasks;

namespace Orderino.Server.Controllers
{
    public class AzureBlobUploadController : Controller
    {
        private readonly IAzureBlobUploadService azureBlobService;

        public AzureBlobUploadController(IAzureBlobUploadService azureBlobService)
        {
            this.azureBlobService = azureBlobService;
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromBody] string imageData)
        {
            var imageDto = await azureBlobService.UploadImage(imageData);
            return Ok(imageDto);
        }

        [HttpPost("upload-banner")]
        public async Task<IActionResult> UploadBanner([FromBody] string imageData)
        {
            var imageDto = await azureBlobService.UploadBanner(imageData);
            return Ok(imageDto);
        }
    }
}
