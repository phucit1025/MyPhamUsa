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
        ICollection<ProductViewModel> GetProducts(int categoryId);
        ProductViewModel GetProduct(int id);
        bool UpdateProduct(ProductUpdateViewModel newProduct);
        bool DeleteProduct(int id);
        bool CreateProduct(ProductCreateViewModel newProduct);
        bool RenewQuantityIndex(int productId);
        bool UpdateProduct(ProductStaffUpdateViewModel newProduct);

        #region For Client
        ICollection<ClientProductViewModel> GetClientProducts();
        ClientProductViewModel GetClientProduct(int id);
        ICollection<ClientProductViewModel> GetClientProducts(int categoryId);
        #endregion
    }
}
