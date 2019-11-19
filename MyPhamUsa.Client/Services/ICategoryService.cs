using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MyPhamUsa.Client.Data;
using MyPhamUsa.Client.Models.Entities;
using MyPhamUsa.Client.Models.ViewModels;

namespace MyPhamUsa.Client.Services
{
    public interface ICategoryService
    {
        ICollection<CategoryViewModel> GetCategories();
    }

    public class CategorySerivce : ICategoryService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public CategorySerivce(AppDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ICollection<CategoryViewModel> GetCategories()
        {
            var results = new List<CategoryViewModel>();
            var categories = _dbContext.Categories.Where(c => !c.IsDeleted).OrderBy(c => c.Name);
            results.AddRange(_mapper.Map<List<Category>, List<CategoryViewModel>>(categories.ToList()));
            return results;
        }
    }
}
