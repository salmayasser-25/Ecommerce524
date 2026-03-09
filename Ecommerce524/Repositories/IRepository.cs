using System.Linq.Expressions;

namespace Ecommerce524.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task CreateAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

        // Added params for Include
        Task<List<T>> GetAsync(
            Expression<Func<T, bool>>? filter = null,
            bool tracked = true,
            params Expression<Func<T, object>>[] includes);

        Task<T?> GetOneAsync(
            Expression<Func<T, bool>>? filter = null,
            bool tracked = true,
            params Expression<Func<T, object>>[] includes);

        Task<T?> GetByIdAsync(int id);

        Task<int> SaveChangesAsync();
    }
}