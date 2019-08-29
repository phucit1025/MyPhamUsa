using MyPhamUsa.Models.ViewModels;
using System.Collections.Generic;

namespace MyPhamUsa.Services.Interfaces
{
    public interface IImageService
    {
        ICollection<ImageViewModel> GetImages(int productId);
        bool DeleteImage(int id);
        bool UpdateImage(ImageUpdateViewModel updateModel);
    }
}
