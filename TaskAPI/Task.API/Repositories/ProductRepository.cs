using Task.API.Data;
using Task.API.Models;
using Task.API.Repositories.IRepository;

namespace Task.API.Repositories
{
    public class ProductRepository :Repository<Product> , IProductRepository
    {
        private readonly TaskApiDbContext dbContext;
        public ProductRepository(TaskApiDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
    
}
