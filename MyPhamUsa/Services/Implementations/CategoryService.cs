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
            var categories = _context.Categories.Where(c => !c.IsDeleted).ToList();
            var results = _mapper.Map<List<Category>, List<CategoryViewModel>>(categories);
            return results;
        }

        public bool UpdateCategory(CategoryViewModel newCategory)
        {
            var category = _context.Categories.Find(newCategory.Id);
            var result = _mapper.Map<CategoryViewModel, Category>(newCategory, category);
            try
            {
                _context.Update(result);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }

        }

        private void DeepDelete(Category category)
        {
            if (category.Childs.Count != 0)
            {
                foreach (var child in category.Childs)
                {
                    DeepDelete(child);
                }
            }
            category.IsDeleted = true;
            _context.Update(category);
            _context.SaveChanges();
        }
    }
}
