using ArabTube.Entities.VideoModels;
using ArabTube.Services.VideoServices.Interfaces;
using FFmpeg.NET;
using FFmpeg.NET.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;


namespace ArabTube.Services.VideoServices.ImplementationClasses
{
    public class VideoService : IVideoService
    {
        private readonly IConfiguration _configuration;
        private readonly string _tempPath;
        private readonly Engine _ffmpegEngine;
        public VideoService(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._tempPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Videos");
            //this._ffmpegEngine = new Engine(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\ffmpeg", "ffmpeg.exe"));
        }

        public async Task<IEnumerable<VideoQuality>> ProcessVideoAsync(ProcessingVideo model)
        {
            var filePath = Path.Combine(_tempPath, model.Title + Path.GetExtension(model.Video.FileName));
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Video.CopyToAsync(stream);
            }

            var paths = await ProcessVideoQualityAsync(filePath, model.Title, model.Username);
            File.Delete(filePath);
            return paths;
        }

        private async Task<IEnumerable<VideoQuality>> ProcessVideoQualityAsync(string filePath, string title, string username)
        {
            var inputFile = new InputFile(filePath);
            (int width, int height)[] resolutions = new (int, int)[]
            {
                (256, 144),
                (426, 240),
                (640, 360),
                (854, 480),
                (1280,720)
            };

            List<VideoQuality> videoQualities = new List<VideoQuality>();

            foreach (var resolution in resolutions)
            {
                var outputFilePath = Path.Combine(_tempPath, Guid.NewGuid().ToString() + ".mp4");
                var outputFile = new OutputFile(outputFilePath);
                var output = await _ffmpegEngine.ConvertAsync(inputFile, outputFile, new ConversionOptions
                {
                    VideoSize = VideoSize.Custom,
                    CustomHeight = resolution.height,
                    CustomWidth = resolution.width
                }, default).ConfigureAwait(false);

                var videoQuality = new VideoQuality
                {
                    BlobName = $"{title}-{resolution.height}",
                    Path = outputFilePath,
                    ContentType = "video/mp4",
                    ContainerName = username
                };
                videoQualities.Add(videoQuality);

            }

            return videoQualities;
        }

    }
}
