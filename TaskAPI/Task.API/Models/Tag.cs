using System.ComponentModel.DataAnnotations;

namespace Task.API.Models
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }
        public string TagName { get; set; }

        public ICollection<ProductTag> ProductTags { get; set; } 

    }
}
