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
        public async Task<Category> UpdateAsync(Category category)
        {
            dbContext.Categories.Update(category);
            await dbContext.SaveChangesAsync();
            return category;
        }
    }
    
}
