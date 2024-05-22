using ArabTube.Entities.VideoModels;
using ArabTube.Services.ControllersServices.CloudServices.Interfaces;
using Azure.Core;
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

namespace ArabTube.Services.ControllersServices.CloudServices.ImplementationClasses
{
    public class CloudService : ICloudService
    {
        private readonly IConfiguration _configuration;
        public CloudService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task UploadToCloudAsync(IEnumerable<VideoQuality> videoQualities)
        {
            foreach (var videoQuality in videoQualities)
            {
                var options = new BlobClientOptions
                {
                    Retry =
                    {
                        Mode = RetryMode.Exponential,
                        MaxRetries = 3,
                        Delay = TimeSpan.FromSeconds(10),
                        MaxDelay = TimeSpan.FromSeconds(60),
                        NetworkTimeout = TimeSpan.FromMinutes(5)
                    }
                };
                var blobAccount = new BlobServiceClient(
                    new Uri($"{_configuration["BlobStorage:ConnectionString"]}?{_configuration["BlobStorage:SAS"]}"), options);
                var containerClient = blobAccount.GetBlobContainerClient(videoQuality.ContainerName);
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
                var blobClient = containerClient.GetBlobClient(videoQuality.BlobName);

                StorageTransferOptions transferOptions = new StorageTransferOptions
                {
                    InitialTransferSize = 10 * 1024 * 1024,
                    MaximumConcurrency = 10,
                    MaximumTransferSize = 10 * 1024 * 1024
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
