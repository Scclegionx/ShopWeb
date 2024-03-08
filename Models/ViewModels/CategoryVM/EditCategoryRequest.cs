﻿using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models.ViewModels.CategoryVM
{
    public class EditCategoryRequest
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
