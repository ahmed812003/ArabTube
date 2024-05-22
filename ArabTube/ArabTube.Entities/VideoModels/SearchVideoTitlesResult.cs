using ArabTube.Entities.GenericModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.VideoModels
{
    public class SearchVideoTitlesResult : ProcessResult
    {
        public IEnumerable<string> Titles { get; set; } = new List<string>();
    }
}
