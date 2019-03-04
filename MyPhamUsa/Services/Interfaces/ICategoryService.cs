using MyPhamUsa.Models.ViewModels;
using System.Collections.Generic;

namespace MyPhamUsa.Services.Interfaces
{
    public interface ICategoryService
    {
        ICollection<CategoryViewModel> GetCategories();
        bool CreateCategory(CategoryCreateViewModel newCategory);
        bool UpdateCategory(CategoryViewModel newCategory);
        bool DeleteCategory(int id);
    }
}
