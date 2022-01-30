using Orderino.Infrastructure.AzureBlob.Interfaces;
using Orderino.Shared.DTOs;
using Orderino.Shared.Models.Enums;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.AzureBlob
{
    public class AzureBlobUploadService : IAzureBlobUploadService
    {
        private readonly IAzureBlobConnectionFactory azureBlobConnectionFactory;

        public AzureBlobUploadService(IAzureBlobConnectionFactory azureBlobConnectionFactory)
        {
            this.azureBlobConnectionFactory = azureBlobConnectionFactory;
        }

        public async Task<ImageDto> UploadImage(string imageData)
        {
            if (string.IsNullOrEmpty(imageData))
                return new ImageDto { ImageUrl = string.Empty };

            byte[] bytes = Convert.FromBase64String(imageData);
            var blobContainer = await azureBlobConnectionFactory.GetBlobContainer(ImageType.Product);

            var blob = blobContainer.GetBlockBlobReference(GetRandomBlobName());
            using (var stream = new MemoryStream(bytes, 0, bytes.Length))
            {
                await blob.UploadFromStreamAsync(stream);
            }

            return new ImageDto
            {
                ImageUrl = blob.Uri.AbsoluteUri
            };
        }

        public async Task<ImageDto> UploadBanner(string imageData)
        {
            if (string.IsNullOrEmpty(imageData))
                return new ImageDto { ImageUrl = string.Empty };

            byte[] bytes = Convert.FromBase64String(imageData);
            var blobContainer = await azureBlobConnectionFactory.GetBlobContainer(ImageType.Banner);

            var blob = blobContainer.GetBlockBlobReference(GetRandomBlobName());
            using (var stream = new MemoryStream(bytes, 0, bytes.Length))
            {
                await blob.UploadFromStreamAsync(stream);
            }

            return new ImageDto
            {
                ImageUrl = blob.Uri.AbsoluteUri
            };
        }

        private string GetRandomBlobName()
        {
            var guid = Guid.NewGuid();
            string guidString = Convert.ToBase64String(guid.ToByteArray());
            guidString = guidString.Replace("=", "");
            guidString = guidString.Replace("+", "");

            return $"{guidString}-{DateTime.Now.Ticks}.png";
        }       
    }
}
