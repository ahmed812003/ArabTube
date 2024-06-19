using ArabTube.Entities.AuthModels;
using ArabTube.Entities.GenericModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.UserModels
{
    public class SecurityResult : ProcessResult
    {
        public SecurityResponse SecurityResponse { get; set; } = new SecurityResponse();
    }
}
