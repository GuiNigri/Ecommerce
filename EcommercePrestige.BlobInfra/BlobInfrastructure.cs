using Azure.Storage.Blobs;
using EcommercePrestige.Model.Interfaces.Infrastructure;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EcommercePrestige.Blob
{
    public class BlobInfrastructure: IBlobInfrastructure
    {
        private readonly BlobServiceClient _blobClient;

        public BlobInfrastructure(string storageAccount)
        {
            _blobClient = new BlobServiceClient(storageAccount);
        }

        public async Task<string> CreateBlobAsync(Stream stream, string container)
        {
            var containerClient = _blobClient.GetBlobContainerClient(container);

            if (!await containerClient.ExistsAsync())
            {
                await containerClient.CreateIfNotExistsAsync();
                await containerClient.SetAccessPolicyAsync(global::Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            }

            var blobClient = containerClient.GetBlobClient($"{Guid.NewGuid()}.jpg");


            await blobClient.UploadAsync(stream, true);

            return blobClient.Uri.ToString();
        }


        public async Task DeleteBlobAsync(string blobName, string container)
        {
            var containerClient = _blobClient.GetBlobContainerClient(container);

            var blob = new BlobClient(new Uri(blobName));

            var blobClient = containerClient.GetBlobClient(blob.Name);

            await blobClient.DeleteIfExistsAsync();
        }
    }
}
