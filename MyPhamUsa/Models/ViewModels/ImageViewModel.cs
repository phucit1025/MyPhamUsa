using System.Collections.Generic;

namespace MyPhamUsa.Models.ViewModels
{
    public class ImageViewModel
    {
        public int Id { get; set; }
        public string Path { get; set; }
    }

    public class ImageUpdateViewModel
    {
        public int ProductId { get; set; }
        public List<string> Base64Images { get; set; }
        public List<int> DeleteImages { get; set; }
    }
}
