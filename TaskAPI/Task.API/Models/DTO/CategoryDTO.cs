using System.ComponentModel.DataAnnotations;

namespace Task.API.Models.DTO
{
    public class CategoryDTO
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
