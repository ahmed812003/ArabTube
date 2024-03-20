using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.AuthModels
{
    public class AuthResult
    {
        public AuthResult()
        {
            //To avoid NULL Exception
            Roles = new List<string>();
        }

        public string message { get; set; }

        public bool IsAuthenticated { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public List<string> Roles { get; set; }

        public string Token { get; set; }

        public DateTime ExpireOn { get; set; }

    }
}
