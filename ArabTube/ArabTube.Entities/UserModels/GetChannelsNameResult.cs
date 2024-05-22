using ArabTube.Entities.GenericModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.UserModels
{
    public class GetChannelsNameResult : ProcessResult
    {
        public IEnumerable<string> names { get; set; } = new List<string>();
    }
}
