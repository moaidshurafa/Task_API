using System.ComponentModel.DataAnnotations;

namespace Task.API.Models.DTO
{
    public class ProductDTO
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public List<string> Tags { get; set; }

    }
}
