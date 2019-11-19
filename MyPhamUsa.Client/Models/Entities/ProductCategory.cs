using System.ComponentModel.DataAnnotations.Schema;

namespace MyPhamUsa.Client.Models.Entities
{
    public class ProductCategory : BaseEntity
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
