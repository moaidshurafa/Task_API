using System.ComponentModel.DataAnnotations;

namespace Task.API.Models.DTO
{
    public class TagDTO
    {
        [Key]
        public int TagId { get; set; }
        public string TagName { get; set; }
    }
}
