using System.ComponentModel.DataAnnotations.Schema;

namespace MyPhamUsa.Models.Entities
{
    public class Image : BaseEntity
    {
        public string Path { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
