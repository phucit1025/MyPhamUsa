﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPhamUsa.Models.Entities
{
    public class Product : BaseEntity
    {
        public Product()
        {
            ProductCategories = new HashSet<ProductCategory>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string SellPrice { get; set; }
        public int QuantityIndex { get; set; }

        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
