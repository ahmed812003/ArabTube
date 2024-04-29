using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.VideoModels
{
    public class ProcessingVideo
    {
        public string Username { get; set; }

        public string Title { get; set; }

        public IFormFile Video { get; set; }
    }
}
