using ArabTube.Entities.DtoModels.UserDTOs;
using ArabTube.Entities.GenericModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.UserModels
{
    public class GetUsersResult : ProcessResult
    {
        public IEnumerable<UserViewDto> users { get; set; } = new List<UserViewDto>();
    }
}
