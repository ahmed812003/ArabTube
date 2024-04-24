using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FFmpeg.NET;
using FFmpeg.NET.Enums;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using Engine = FFmpeg.NET.Engine;

namespace ArabTube.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _tempPath;
        public VideosController(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._tempPath = Path.GetTempPath();
        }

        [HttpGet()]
        public async Task<IActionResult> Get(string name)
        {
            var blobClient = new BlobClient(new Uri($"https://arabtubeapp.blob.core.windows.net/thumbnails/{name}.jpg"));
            var photo = await blobClient.DownloadContentAsync();

            return Ok(photo.Value.Content.ToArray());
        }

        [HttpPost("UploadVideo")]
        public async Task<IActionResult> AddVideoToBlobStorage(IFormFile video)
        {
            if (video == null || video.Length == 0)
                return BadRequest("No video file provided.");

            #region  change quality
            var videoName = video.FileName.Replace(".mp4", "");

            var filePath = Path.Combine(_tempPath, videoName + Path.GetExtension(video.FileName));
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await video.CopyToAsync(stream);
            }

            var ffmpeg = new Engine(@"C:\ffmpeg\ffmpeg.exe");
            var inputFile = new FFmpeg.NET.InputFile(filePath);
            var metaDate = await ffmpeg.GetMetaDataAsync(inputFile, default).ConfigureAwait(false);
            (int width, int height)[] resolutions = new (int, int)[]
            {
                (256, 144),
                (426, 240),
                (640, 360),
                (854, 480),
                (1280,720)
            };

            foreach (var resolution in resolutions)
            {
                var outputFilePath = Path.Combine(_tempPath, Guid.NewGuid().ToString() + ".mp4");
                var outputFile = new OutputFile(outputFilePath);
                var output = await ffmpeg.ConvertAsync(inputFile, outputFile, new ConversionOptions
                {
                    VideoSize = VideoSize.Custom,
                    CustomHeight = resolution.height,
                    CustomWidth = resolution.width
                }, default).ConfigureAwait(false);

                #region Upload To Cloud 
                string blobName = $"{videoName}-{resolution.height}";
                string containerClient = "videos";
                string contentType = "video/mp4";
                await UploadToCloud(blobName, containerClient, contentType, outputFilePath);
                System.IO.File.Delete(outputFilePath);
                #endregion
            }
            System.IO.File.Delete(filePath);
            return Ok("Video uploaded and encoded successfully.");
            #endregion
        }

        private async Task<bool> UploadToCloud(string blobName, string containerClient,
            string contentType, string outputFilePath)
        {
            var BlobAccount = new BlobServiceClient(new Uri(_configuration["BlobStorage:ConnectionString"]),
                new DefaultAzureCredential());
            var ContainerClient = BlobAccount.GetBlobContainerClient(containerClient);
            await ContainerClient.CreateIfNotExistsAsync();
            var BlobClient = ContainerClient.GetBlobClient(blobName);

            StorageTransferOptions transferOptions = new StorageTransferOptions
            {
                InitialTransferSize = 10 * 1024 * 1024,
                MaximumConcurrency = 5,
                MaximumTransferSize = 10 * 1024 * 1024
            };
            var headers = new BlobHttpHeaders
            {
                ContentType = contentType
            };
            BlobUploadOptions blobUploadOptions = new BlobUploadOptions
            {
                TransferOptions = transferOptions,
                HttpHeaders = headers
            };
            await BlobClient.UploadAsync(outputFilePath, blobUploadOptions);
            return true;
        }

    }
}
