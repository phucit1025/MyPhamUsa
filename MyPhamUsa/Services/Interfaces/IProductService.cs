using MyPhamUsa.Models.ViewModels;

namespace MyPhamUsa.Services.Interfaces
{
    public interface IProductService
    {
        ProductOfStaffPagingViewModel GetProductsByStaff(int pageSize, int pageIndex);
        ProductViewModel GetProduct(int id);
        bool UpdateProduct(ProductUpdateViewModel newProduct);
        bool DeleteProduct(int id);
        bool CreateProduct(ProductCreateViewModel newProduct);
        bool RenewQuantityIndex(int productId);
        bool UpdateProduct(ProductStaffUpdateViewModel newProduct);
        bool IsAvailableCode(ProductCodeValidViewModel code);
        ProductPagingViewModel GetProducts(int pageSize, int pageIndex);
        ProductPagingViewModel GetProducts(int categoryId, int pageSize, int pageIndex);
        ProductPagingViewModel SearchProducts(string name, int pageSize, int pageIndex);
    }
}
