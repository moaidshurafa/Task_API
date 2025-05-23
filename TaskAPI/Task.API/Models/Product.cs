﻿using System.ComponentModel.DataAnnotations;

namespace Task.API.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<ProductTag> ProductTags { get; set; }

    }
}
