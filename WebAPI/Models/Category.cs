﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ValidateNever]
        public ICollection<Product> Products { get; set; }
    }
}
