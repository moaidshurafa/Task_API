using Task.API.Models;

namespace Task.API.Repositories.IRepository
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<Tag> UpdateAsync(Tag tag);

    }
}
