using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.VideoModels
{
    public class VideoQuality
    {
        public string BlobName { get; set; }

        public string ContainerName { get; set; }

        public string ContentType { get; set; }

        public string Path { get; set; }
    }
}
