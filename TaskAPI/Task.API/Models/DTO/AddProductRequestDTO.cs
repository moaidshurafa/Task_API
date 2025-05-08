namespace Task.API.Models.DTO
{
    public class AddProductRequestDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public List<int> TagIds { get; set; }

    }
}
