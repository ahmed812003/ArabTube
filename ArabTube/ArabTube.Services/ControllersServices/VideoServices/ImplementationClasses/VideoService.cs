using ArabTube.Entities.DtoModels.VideoDTOs;
using ArabTube.Entities.GenericModels;
using ArabTube.Entities.Models;
using ArabTube.Entities.VideoModels;
using ArabTube.Services.ControllersServices.CloudServices.Interfaces;
using ArabTube.Services.ControllersServices.VideoServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using AutoMapper;
using FFmpeg.NET;
using FFmpeg.NET.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ArabTube.Services.ControllersServices.VideoServices.ImplementationClasses
{
    public class VideoService : IVideoService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICloudService _cloudService;
        private readonly string _tempPath;
        private readonly Engine _ffmpegEngine;
        private readonly UserManager<AppUser> _userManager;
        public VideoService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper, ICloudService cloudService, UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _tempPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Videos");
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudService = cloudService;
            _userManager = userManager;
            //this._ffmpegEngine = new Engine(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\ffmpeg", "ffmpeg.exe"));
            this._ffmpegEngine = new Engine("C:\\ffmpeg\\ffmpeg.exe");
        }

        public async Task<SearchVideoTitlesResult> SearchVideoTitlesAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new SearchVideoTitlesResult { Message = "Query Cannot Be Empty" };
            }

            var titles = await _unitOfWork.Video.SearchVideoTitlesAsync(query);

            if (!titles.Any())
            {
                return new SearchVideoTitlesResult { Message = "No Titles Found Matching The Search Query" };
            }

            return new SearchVideoTitlesResult { IsSuccesed = true, Titles = titles };
        }

        public async Task<GetVideoResult> SearchVideoAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new GetVideoResult { Message = "Query Cannot Be Empty" };
            }

            var videos = await _unitOfWork.Video.SearchVideoAsync(query);

            if (!videos.Any())
            {
                return new GetVideoResult { Message = "No Videos Found Matching The Search Query" };
            }

            IEnumerable<VideoPreviewDto> viewVideosDtoList = _mapper.Map<IEnumerable<VideoPreviewDto>>(videos);
            return new GetVideoResult { IsSuccesed = true , Videos = viewVideosDtoList};
        }

        public async Task<GetVideoResult> PreviewVideoAsync()
        {
            var videos = await _unitOfWork.Video.GetAllAsync();
            if (!videos.Any())
            {
                return new GetVideoResult { Message = "No Videos Found " };
            }
            IEnumerable<VideoPreviewDto> viewVideosDtoList = _mapper.Map<IEnumerable<VideoPreviewDto>>(videos);

            return new GetVideoResult { IsSuccesed = true , Videos = viewVideosDtoList};
        }

        public async Task<WatchVideoResult> WatchVideoAsync(string id)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(id);

            if (video == null)
            {
                return new WatchVideoResult { Message = $"Video With id {id} does not exist!" };
            }

            var viewVideo = _mapper.Map<ViewVideoDto>(video);
            return new WatchVideoResult { IsSuccesed = true , Video = viewVideo};
        }

        public async Task<GetVideoResult> UserVideosAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return new GetVideoResult { Message = $"No user exist with id = {userId}" };
            }
            var videos = await _unitOfWork.Video.GetUserVideos(userId);
            if (!videos.Any())
            {
                return new GetVideoResult { Message = "User Dosn't upload any videos" };
            }
            IEnumerable<VideoPreviewDto> viewVideosDtoList = _mapper.Map<IEnumerable<VideoPreviewDto>>(videos);
            return new GetVideoResult
            {
                IsSuccesed = true,
                Videos = viewVideosDtoList
            };
        }
        
        public async Task<ProcessResult> IsUserLikeVideoAsync(string videoId , string userId)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(videoId);
            if(video == null)
            {
                return new ProcessResult { Message = $"No video exists with id = {videoId}" };
            }
            var isUserLike = await _unitOfWork.VideoLike.CheckUserLikeVideoOrNotAsync(userId, videoId);
            return new ProcessResult
            {
                IsSuccesed = true,
                Message = isUserLike ? "YES" : "NO"
            };
        }

        public async Task<ProcessResult> IsUserDisLikeVideoAsync(string videoId, string userId)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(videoId);
            if (video == null)
            {
                return new ProcessResult { Message = $"No video exists with id = {videoId}" };
            }
            var isUserDisLike = await _unitOfWork.VideoDislike.CheckUserDislikeVideoOrNotAsync(userId, videoId);
            return new ProcessResult
            {
                IsSuccesed = true,
                Message = isUserDisLike ? "YES" : "NO"
            };
        }

        public async Task<ProcessResult> IsUserFlagVideoAsync(string videoId, string userId)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(videoId);
            if (video == null)
            {
                return new ProcessResult { Message = $"No video exists with id = {videoId}" };
            }
            var isUserFlag = await _unitOfWork.VideoFlag.CheckUserFlagVideoOrNotAsync(userId, videoId);
            return new ProcessResult
            {
                IsSuccesed = true,
                Message = isUserFlag ? "YES" : "NO"
            };
        }

        public async Task<ProcessResult> UploadVideoAsync(UploadingVideoDto model, string userName , AppUser user)
        {
            if (model.Video.ContentType != "video/mp4")
            {
                return new ProcessResult { Message = "Video Type is Not Mp4" };
            }
            var video = _mapper.Map<Video>(model);
            var processingVideo = new ProcessingVideo
            {
                Username = userName,
                Video = model.Video,
                Title = video.Id
            };

            /*var encodedVideos = await ProcessVideoAsync(processingVideo);
            await _cloudService.UploadToCloudAsync(encodedVideos);*/

            
            video.UserId = user.Id;
            string uri = new Uri(_configuration["BlobStorage:ConnectionString"]).ToString();
            video.VideoUri = $"{uri}{userName}/{video.Id}-";

            var result = await _unitOfWork.Video.AddAsync(video);
            if (!result)
            {
                return new ProcessResult { Message = "Failed to Upload video" };
            }
            user.NumberOfvideos += 1;
            
            var followers = await _unitOfWork.AppUserConnection.GetFollowersAsync(user.Id);
            foreach (var follower in followers)
            {
                var notification = new Notification
                {
                    Message = $"{user.FirstName} {user.LastName} Upload new video",
                    UserId = follower.FollowerId,
                    SenderId = user.Id,
                    VideoId = video.Id
                };
                await _unitOfWork.Notification.AddAsync(notification);
            }

            await _unitOfWork.Complete();
            return new ProcessResult { IsSuccesed = true };
        }

        public async Task<ProcessResult> LikeVideoAsync(string id, string userId)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(id);
            if (video == null)
            {
                return new ProcessResult { Message = $"Failed to like video With id {id}!" };
            }
            // user first like => increase Likes with 1 , Add video to likes playlist , add {user.id , video.id} to videosLikes 
            // user second Like => mean that he erase the like so, decrease likes , remove video from likes playlist , remove {user.id , video.id} from videosLikes
            var IsUserLikeVideo = await _unitOfWork.VideoLike.CheckUserLikeVideoOrNotAsync(userId, id);
            if (IsUserLikeVideo)
            {
                var isLikeRemoved = await _unitOfWork.VideoLike.RemoveUserVideoLikeAsync(userId, id);
                if(!isLikeRemoved)
                {
                    return new ProcessResult { Message = $"Failed to remove like video With id {id}!" };
                }
                video.Likes -= 1;
            }
            else
            {
                var videoLike = new VideoLike
                {
                    UserId = userId,
                    VideoId = id,
                };
                var isLikeAdded = await _unitOfWork.VideoLike.AddAsync(videoLike);
                if(!isLikeAdded)
                {
                    return new ProcessResult { Message = $"Failed to add like video With id {id}!" };
                }
                video.Likes += 1;
            }
            await _unitOfWork.Complete();
            return new ProcessResult 
            { 
                IsSuccesed = true,
                Message = IsUserLikeVideo ? "Remove" : "Add"
            };
        }

        public async Task<ProcessResult> DislikeVideoAsync(string id, string userId)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(id);
            if (video == null)
            {
                return new ProcessResult { Message = $"Failed to Dislike video With id {id}!" };
            }
            var IsUserDislikeVideo = await _unitOfWork.VideoDislike.CheckUserDislikeVideoOrNotAsync(userId , id);
            if (IsUserDislikeVideo)
            {
                var isDislikeRemoved = await _unitOfWork.VideoDislike.RemoveUserVideoDislikeAsync(userId , id);
                if (!isDislikeRemoved)
                {
                    return new ProcessResult { Message = $"Failed to remove Dislike video With id {id}!" };
                }
                video.DisLikes -= 1;
            }
            else
            {
                var videoDislike = new VideoDislike
                {
                    UserId = userId,
                    VideoId = id,
                };
                var isDislikeAdded = await _unitOfWork.VideoDislike.AddAsync(videoDislike);
                if(!isDislikeAdded)
                {
                    return new ProcessResult { Message = $"Failed to Dislike video With id {id}!" };
                }
                video.DisLikes += 1;
            }
            await _unitOfWork.Complete();
            return new ProcessResult { IsSuccesed = true };
        }

        public async Task<ProcessResult> FlagVideoAsync(string id , string userId)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(id);
            if (video == null)
            {
                return new ProcessResult { Message = $"Video With id {id} does not exist!" };
            }
            var isuserFlagVideo = await _unitOfWork.VideoFlag.CheckUserFlagVideoOrNotAsync(userId, id);
            if(isuserFlagVideo)
            {
                var IsFlagRemoved = await _unitOfWork.VideoFlag.RemoveUserVideoFlagAsync(userId, id);
                if (!IsFlagRemoved)
                {
                    return new ProcessResult { Message = $"Failed to remove flag video With id {id}!" };
                }
                video.Flags -= 1;
            }
            else
            {
                var videoFlag = new VideoFlag
                {
                    UserId = userId,
                    VideoId = id,
                };
                var isFlagAdded = await _unitOfWork.VideoFlag.AddAsync(videoFlag);
                if(!isFlagAdded)
                {
                    return new ProcessResult { Message = $"Failed to flag video With id {id}!" };
                }
                video.Flags += 1;
            }
            await _unitOfWork.Complete();
            return new ProcessResult { IsSuccesed = true};
        }

        public async Task<ProcessResult> ViewVideoAsync(string id)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(id);
            if (video == null)
            {
                return new ProcessResult { Message = $"Video With id {id} does not exist!" };
            }
            video.Views += 1;
            await _unitOfWork.Complete();
            return new ProcessResult { IsSuccesed = true};
        }

        public async Task<ProcessResult> UpdateVideoAsync(UpdatingVideoDto updateDto, string videoId, string userId)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(videoId);
            if (video == null)
            {
                return new ProcessResult { Message = $"No Video exists with id = {videoId}" };
            }

            if (video.UserId != userId)
            {
                return new ProcessResult { Message = $"Unauthorized to update this video" };
            }

            //var updatedVideo = _mapper.Map<Video>(updateDto);
            _mapper.Map(updateDto , video);
            _unitOfWork.Video.Update(video);
            await _unitOfWork.Complete();
            return new ProcessResult{ IsSuccesed = true};
        }
        
        public async Task<ProcessResult> DeleteAsync(string id, AppUser user)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(id);
            if (video == null)
            {
                return new ProcessResult { Message = $"No Video exists with id = {id}" };
            }

            if(video.UserId != user.Id)
            {
                return new ProcessResult { Message = $"Unauthorized to delete this video" };
            }

            var isDeleted = await _unitOfWork.Video.DeleteAsync(id);
            if (!isDeleted)
            {
                return new ProcessResult { Message = "Error While Deleting video"};
            }
            user.NumberOfvideos -= 1;
            await _unitOfWork.Complete();
            return new ProcessResult { IsSuccesed = true};
        }

        private async Task<IEnumerable<VideoQuality>> ProcessVideoAsync(ProcessingVideo model)
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
