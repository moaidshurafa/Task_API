using Task.API.Models;

namespace Task.API.Repositories.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> UpdateAsync(Product product);

    }
}
