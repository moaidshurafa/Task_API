﻿using System.ComponentModel.DataAnnotations;

namespace Task.API.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ICollection<Product> Products { get; set; }

    }
}
