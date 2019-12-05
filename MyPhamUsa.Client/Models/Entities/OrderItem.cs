using System.ComponentModel.DataAnnotations.Schema;

namespace MyPhamUsa.Client.Models.Entities
{
    public class OrderItem : BaseEntity
    {
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public string OrderPrice { get; set; }
        public long StorageId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

    }
}
