using Orderino.Shared.DTOs;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.AzureBlob.Interfaces
{
    public interface IAzureBlobUploadService
    {
        Task<ImageDto> UploadImage(string imageData);

        Task<ImageDto> UploadBanner(string imageData);
    }
}
