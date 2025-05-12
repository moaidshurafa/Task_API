using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using Task.API.Repositories.IRepository;
using Task.API.Data;
using StdTask = System.Threading.Tasks.Task;
namespace Task.API.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly TaskApiDbContext dbContext;
        internal DbSet<T> dbSet;

        public Repository(TaskApiDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = this.dbContext.Set<T>();
        }

        public async StdTask AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async StdTask RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async StdTask SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
