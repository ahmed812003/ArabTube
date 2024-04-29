using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IVideoRepository Video { get; }
    }
}
