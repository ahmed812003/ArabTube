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
