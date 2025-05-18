using System.Linq.Expressions;
using StdTask = System.Threading.Tasks.Task;
namespace Task.API.Repositories.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        StdTask AddAsync(T entity);
        StdTask UpdateAsync(T entity);
        StdTask RemoveAsync(T entity);
        StdTask SaveAsync();
    }
}
