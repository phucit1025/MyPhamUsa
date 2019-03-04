using System.Collections.Generic;

namespace MyPhamUsa.Models.Entities
{
    public class Product : BaseEntity
    {
        public Product()
        {
            ProductCategories = new HashSet<ProductCategory>();
            Images = new HashSet<Image>();
        }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string SellPrice { get; set; }
        public int QuantityIndex { get; set; }

        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}
