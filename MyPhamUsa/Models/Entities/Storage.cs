using System.ComponentModel.DataAnnotations.Schema;

namespace MyPhamUsa.Models.Entities
{
    public class Storage : BaseEntity
    {
        public int ProductId { get; set; }
        public int? OrderId { get; set; }
        public int Quantity { get; set; }
        public bool IsIssued { get; set; }
        public string Description { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
