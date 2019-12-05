using MyPhamUsa.Client.Enums;
using System.Collections.Generic;

namespace MyPhamUsa.Client.Models.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {
            Items = new HashSet<OrderItem>();
        }
        public int? UserId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public OrderState State { get; set; }
        public virtual ICollection<OrderItem> Items { get; set; }
    }
}
