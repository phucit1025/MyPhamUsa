using AutoMapper;
using MyPhamUsa.Data;
using MyPhamUsa.Models.ViewModels;
using MyPhamUsa.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyPhamUsa.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyPhamUsa.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool CreateCategory(CategoryCreateViewModel newCategory)
        {
            var category = _mapper.Map<CategoryCreateViewModel, Category>(newCategory);
            try
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public bool DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                DeepDelete(category);
                return true;
            }
            return false;
        }

        public ICollection<CategoryViewModel> GetCategories()
        {
            throw new NotImplementedException();
        }

        public bool UpdateCategory(CategoryViewModel newCategory)
        {
            throw new NotImplementedException();
        }

        private void DeepDelete(Category category)
        {
            if (category.Childs.Count != 0)
            {
                foreach (var child in category.Childs)
                {

                }
            }
            else
            {

            }
        }
    }
}
