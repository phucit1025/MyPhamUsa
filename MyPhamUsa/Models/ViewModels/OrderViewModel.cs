using System;
using System.Collections.Generic;

namespace MyPhamUsa.Models.ViewModels
{
    public class OrderCreateViewModel
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public List<OrderItemCreateViewModel> OrderItems { get; set; }
    }

    public class OrderItemCreateViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
    }

    public class OrderItemViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public List<string> ImagePaths { get; set; }
    }
}
