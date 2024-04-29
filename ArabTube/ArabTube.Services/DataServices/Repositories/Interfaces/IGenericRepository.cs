using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> FindByIdAsync(string id);

        Task<bool> AddAsync(T entity);

        bool Update(T entity);

        Task<bool> DeleteAsync(string id);
    }
}
