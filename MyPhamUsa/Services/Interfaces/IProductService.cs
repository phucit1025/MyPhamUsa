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
        ICollection<ProductOfStaffViewModel> GetProductsByStaff();
        bool UpdateProduct(ProductUpdateViewModel newProduct);
        bool DeleteProduct(int id);
        bool CreateProduct(ProductCreateViewModel newProduct);
        bool RenewQuantityIndex(int productId);
        ICollection<ClientProductViewModel> GetClientProducts();
    }
}
