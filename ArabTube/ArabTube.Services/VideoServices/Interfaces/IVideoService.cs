using ArabTube.Entities.VideoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.VideoServices.Interfaces
{
    public interface IVideoService
    {
        Task<IEnumerable<VideoQuality>> ProcessVideoAsync(ProcessingVideo model);
    }
}
