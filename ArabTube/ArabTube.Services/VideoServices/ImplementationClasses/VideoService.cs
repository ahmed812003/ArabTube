using ArabTube.Entities.DtoModels.VideoDTOs;
using ArabTube.Entities.Models;
using ArabTube.Entities.VideoModels;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using ArabTube.Services.VideoServices.Interfaces;
using AutoMapper;
using FFmpeg.NET;
using FFmpeg.NET.Enums;
using Microsoft.Extensions.Configuration;

namespace ArabTube.Services.VideoServices.ImplementationClasses
{
    public class VideoService : IVideoService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly string _tempPath;
        private readonly Engine _ffmpegEngine;
        public VideoService(IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _configuration = configuration;
            _tempPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Videos");
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //this._ffmpegEngine = new Engine(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\ffmpeg", "ffmpeg.exe"));
        }

        public async Task<bool> AddAsync(UploadingVideoDto videoDto, string userName)
        {
            var video =  _mapper.Map<Video>(videoDto);

            // Mapping Video Uri Seperately
            string uri = new Uri(_configuration["BlobStorage:ConnectionString"]).ToString();
            video.VideoUri = $"{uri}{userName}/{video.Id}-";

            var result = await _unitOfWork.Video.AddAsync(video);
            if (!result)
            {
                return false;
            }
            await _unitOfWork.Complete();
      
            return true;
        }

        public async Task<ViewVideoDto> FindByIdAsync(string id)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(id);
            var viewVideo = _mapper.Map<ViewVideoDto>(video);
            return viewVideo;
        }

        public async Task<IEnumerable<VideoPreviewDto>> GetAllAsync()
        {
            var videos = await _unitOfWork.Video.GetAllAsync();
            IEnumerable<VideoPreviewDto> viewVideosDtoList = _mapper.Map<IEnumerable<VideoPreviewDto>>(videos);

            return viewVideosDtoList;
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

        public async Task<IEnumerable<VideoPreviewDto>> SearchVideoAsync(string query)
        {
            var videos = await _unitOfWork.Video.SearchVideoAsync(query);

            IEnumerable <VideoPreviewDto> viewVideosDtoList = _mapper.Map<IEnumerable<VideoPreviewDto>>(videos);
            return viewVideosDtoList;
        }

        public async Task<IEnumerable<string>> SearchVideoTitlesAsync(string query)
        {
            return await _unitOfWork.Video.SearchVideoTitlesAsync(query);
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
