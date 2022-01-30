using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Orderino.Infrastructure.AzureBlob.Interfaces;
using Orderino.Shared.Models.Enums;
using System;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.AzureBlob
{
    public class AzureBlobConnectionFactory : IAzureBlobConnectionFactory
	{
		private readonly IConfiguration configuration;
		private CloudBlobClient blobClient;
		private CloudBlobContainer blobContainer;

		public AzureBlobConnectionFactory(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public async Task<CloudBlobContainer> GetBlobContainer(ImageType imageType)
		{
			if (blobContainer != null)
				return blobContainer;

			var containerName = configuration["ImagesContainerName"];

			if (imageType == ImageType.Banner)
				containerName = configuration["BannersContainerName"];

			if (string.IsNullOrWhiteSpace(containerName))
			{
				throw new ArgumentException("Configuration must contain ContainerName");
			}

			var blobClient = GetClient();

			blobContainer = blobClient.GetContainerReference(containerName);
			if (await blobContainer.CreateIfNotExistsAsync())
			{
				await blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
			}
			return blobContainer;
		}

		private CloudBlobClient GetClient()
		{
			if (blobClient != null)
				return blobClient;

			var storageConnectionString = configuration["StorageConnectionString"];
			if (string.IsNullOrWhiteSpace(storageConnectionString))
			{
				throw new ArgumentException("Configuration must contain StorageConnectionString");
			}

			if (!CloudStorageAccount.TryParse(storageConnectionString, out CloudStorageAccount storageAccount))
			{
				throw new Exception("Could not create storage account with StorageConnectionString configuration");
			}

			blobClient = storageAccount.CreateCloudBlobClient();
			return blobClient;
		}
	}
}
