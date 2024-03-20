using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.AuthModels
{
    public class RegisterResult
    {
        public string message { get; set; }

        public bool IsCreated { get; set; }

        public string Email { get; set; }
    }
}
