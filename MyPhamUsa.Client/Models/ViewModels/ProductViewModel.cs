using System.Collections.Generic;

namespace MyPhamUsa.Client.Models.ViewModels
{
    public class ProductTitleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Image { get; set; }
    }

    public class ProductDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public bool IsInStock { get; set; }

        public ICollection<ImageViewModel> Images { get; set; }
    }
}
