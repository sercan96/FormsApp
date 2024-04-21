﻿using System.ComponentModel.DataAnnotations;

namespace FormsApp.Models.Entities
{
    public class Product
    {
        [Display(Name = "Urun ID")]
        public int ProductId { get; set; }
        [Display(Name = "Urun Adı")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }
        public string Image { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }

        // Relational Properties
        public virtual Category Category { get; set; }


    }
}
