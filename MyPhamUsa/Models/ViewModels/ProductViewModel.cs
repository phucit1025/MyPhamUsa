using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPhamUsa.Models.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string SellPrice { get; set; }
        public int AvailableQuantity { get; set; }
        List<string> ImagePaths { get; set; }
    }

    public class ProductCreateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string SellPrice { get; set; }
        public int ReceiveQuantity { get; set; }
        public List<string> Images { get; set; }
    }
}
