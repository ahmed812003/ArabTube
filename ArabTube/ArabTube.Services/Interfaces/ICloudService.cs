using ArabTube.Entities.VideoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.Interfaces
{
    public interface ICloudService
    {
        Task UploadToCloudAsync(IEnumerable<VideoQuality> videoQualities);
    }
}
