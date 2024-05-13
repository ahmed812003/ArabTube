using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.Models
{
    public class AppUserConnection
    {
        public string FollowerId { get; set; }
        public AppUser Follower { get; set; }

        public string FollowingId { get; set; }
        public AppUser Following { get; set; }

    }
}
