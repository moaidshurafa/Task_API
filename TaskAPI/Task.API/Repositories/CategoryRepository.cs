using Task.API.Data;
using Task.API.Models;
using Task.API.Repositories.IRepository;

namespace Task.API.Repositories
{
    public class CategoryRepository :Repository<Category> , ICategoryRepository
    {
        private readonly TaskApiDbContext dbContext;
        public CategoryRepository(TaskApiDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
        
    }
    
}
