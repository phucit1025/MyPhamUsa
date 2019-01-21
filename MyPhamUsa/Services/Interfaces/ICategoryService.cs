using MyPhamUsa.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
