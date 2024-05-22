using ArabTube.Entities.GenericModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.PlaylistModels
{
    public class SearchPlaylistsTitlesResult : ProcessResult
    {
        public IEnumerable<string> Titles { get; set; } = new List<string>();
    }
}
