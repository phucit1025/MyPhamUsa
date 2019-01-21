using MyPhamUsa.Models.Entities;
using MyPhamUsa.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPhamUsa.Services.Interfaces
{
    public interface IProductService
    {
        ICollection<ProductViewModel> GetProducts();
        bool UpdateProduct(ProductViewModel newProduct);
        bool DeleteProduct(int id);
        bool CreateProduct(ProductCreateViewModel newProduct);
    }
}
