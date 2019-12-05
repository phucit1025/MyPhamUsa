using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPhamUsa.Client.Models.ViewModels
{
    public class OrderCreateViewModel
    {
        public int Id { get; set; }
        public List<OrderItemCreateViewModel> Items { get; set; }
        #region Customer Information
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        #endregion
    }

    public class OrderItemCreateViewModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}
