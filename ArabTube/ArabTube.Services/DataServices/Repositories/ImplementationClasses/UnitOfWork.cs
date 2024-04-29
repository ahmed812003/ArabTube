using ArabTube.Services.DataServices.Data;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.ImplementationClasses
{
    public class UnitOfWork : IUnitOfWork
    {

        public IVideoRepository Video { get; }

        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Video = new VideoRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
