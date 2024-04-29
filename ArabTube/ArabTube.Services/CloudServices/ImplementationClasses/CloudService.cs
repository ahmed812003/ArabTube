using ArabTube.Entities.VideoModels;
using ArabTube.Services.CloudServices.Interfaces;
using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.CloudServices.ImplementationClasses
{
    public class CloudService : ICloudService
    {
        private readonly IConfiguration _configuration;
        public CloudService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task UploadToCloudAsync(IEnumerable<VideoQuality> videoQualities)
        {
            foreach(var videoQuality in videoQualities)
            {
                var blobAccount = new BlobServiceClient(new Uri(_configuration["BlobStorage:ConnectionString"]),
                new DefaultAzureCredential());
                var containerClient = blobAccount.GetBlobContainerClient(videoQuality.ContainerName);
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
                var blobClient = containerClient.GetBlobClient(videoQuality.BlobName);

                StorageTransferOptions transferOptions = new StorageTransferOptions
                {
                    InitialTransferSize = 5 * 1024 * 1024,
                    MaximumConcurrency = 20,
                    MaximumTransferSize = 5 * 1024 * 1024
                };
                var headers = new BlobHttpHeaders
                {
                    ContentType = videoQuality.ContentType
                };
                BlobUploadOptions blobUploadOptions = new BlobUploadOptions
                {
                    TransferOptions = transferOptions,
                    HttpHeaders = headers
                };
                await blobClient.UploadAsync(videoQuality.Path, blobUploadOptions);
                File.Delete(videoQuality.Path);
            }
        }
    }
}
