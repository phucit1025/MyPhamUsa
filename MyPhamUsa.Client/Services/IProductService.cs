using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MyPhamUsa.Client.Data;
using MyPhamUsa.Client.Models.Entities;
using MyPhamUsa.Client.Models.ViewModels;

namespace MyPhamUsa.Client.Services
{
    public interface IProductService
    {
        ICollection<ProductTitleViewModel> GetProducts();
        ICollection<ProductTitleViewModel> GetProducts(int categoryId);
        ProductDetailViewModel GetProduct(int productId);
    }

    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;

        public ProductService(IMapper mapper,AppDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public ProductDetailViewModel GetProduct(int productId)
        {
            var product = _dbContext.Products.Find(productId);
            if (product.IsDeleted) return null;
            var result = _mapper.Map<Product, ProductDetailViewModel>(product);
            return result;
        }

        public ICollection<ProductTitleViewModel> GetProducts()
        {
            var results = new List<ProductTitleViewModel>();
            var products = _dbContext.Products.Where(p => !p.IsDeleted).OrderByDescending(p=>p.DateCreated);
            results.AddRange(_mapper.Map<List<Product>, List<ProductTitleViewModel>>(products.ToList()));
            return results;
        }

        public ICollection<ProductTitleViewModel> GetProducts(int categoryId)
        {
            var results = new List<ProductTitleViewModel>();
            var products = _dbContext.ProductCategories.Where(c => !c.IsDeleted && c.CategoryId == categoryId && !c.Product.IsDeleted).Select(c=>c.Product).OrderByDescending(c => c.DateCreated);
            results.AddRange(_mapper.Map<List<Product>, List<ProductTitleViewModel>>(products.ToList()));
            return results;
        }
    }
}
