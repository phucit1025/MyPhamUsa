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
        public int Price { get; set; }
        public int SellPrice { get; set; }
        public int AvailableQuantity { get; set; }
        public List<string> ImagePaths { get; set; }
    }

    public class ProductCreateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int SellPrice { get; set; }
        public int ReceiveQuantity { get; set; }
        public List<string> Base64Images { get; set; }
    }
}
