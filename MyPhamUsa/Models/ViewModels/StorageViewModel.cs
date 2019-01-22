using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPhamUsa.Models.ViewModels
{
    public class StorageViewModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public bool IsIssued { get; set; }
        public ProductViewModel Product { get; set; }
    }

    public class StorageCreateViewModel
    {

    }

    public class IRViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
    }
}
