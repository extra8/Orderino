using Microsoft.WindowsAzure.Storage.Blob;
using Orderino.Shared.Models.Enums;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.AzureBlob.Interfaces
{
    public interface IAzureBlobConnectionFactory
	{
		Task<CloudBlobContainer> GetBlobContainer(ImageType imageType);
	}
}
