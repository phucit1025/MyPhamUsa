using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPhamUsa.Client.Models.Entities
{
    public class Category : BaseEntity
    {
        public Category()
        {
            Childs = new HashSet<Category>();
            ProductCategories = new HashSet<ProductCategory>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Childs { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
