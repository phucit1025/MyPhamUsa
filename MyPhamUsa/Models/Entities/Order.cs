using System.Collections.Generic;

namespace MyPhamUsa.Models.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {
            OrderItems = new HashSet<Storage>();
        }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Storage> OrderItems { get; set; }
    }
}
