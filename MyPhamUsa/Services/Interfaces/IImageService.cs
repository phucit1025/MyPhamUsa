using System;
using System.Collections.Generic;
using MyPhamUsa.Models.ViewModels;

namespace MyPhamUsa.Services.Interfaces
{
    public interface IImageService
    {
        ICollection<ImageViewModel> GetImages(int productId);
        bool DeleteImage(int id);
    }
}
