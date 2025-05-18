using Task.API.Data;
using Task.API.Models;
using Task.API.Repositories.IRepository;

namespace Task.API.Repositories
{
    public class TagRepository :Repository<Tag> , ITagRepository
    {
        private readonly TaskApiDbContext dbContext;
        public TagRepository(TaskApiDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
        
    }
    
}
