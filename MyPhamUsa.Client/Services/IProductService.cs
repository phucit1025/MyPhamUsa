using AutoMapper;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using MyPhamUsa.Client.Data;
using MyPhamUsa.Client.Models.Entities;
using MyPhamUsa.Client.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyPhamUsa.Client.Services
{
    public interface IProductService
    {
        ICollection<ProductTitleViewModel> GetProducts();
        ICollection<ProductTitleViewModel> GetProducts(int categoryId);
        PagingViewModel SearchProducts(int categoryId, string name, bool asTitle, int pageSize, int pageIndex);
        ProductDetailViewModel GetProduct(int productId);
    }

    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;

        public ProductService(IMapper mapper, AppDbContext dbContext)
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
            var products = _dbContext.Products.Where(p => !p.IsDeleted).OrderByDescending(p => p.DateCreated).AsParallel().ToList();
            results.AddRange(_mapper.Map<List<Product>, List<ProductTitleViewModel>>(products.ToList()));
            return results;
        }

        public ICollection<ProductTitleViewModel> GetProducts(int categoryId)
        {
            var results = new List<ProductTitleViewModel>();
            var products = _dbContext.ProductCategories.Where(c => !c.IsDeleted && c.CategoryId == categoryId && !c.Product.IsDeleted).Select(c => c.Product).OrderByDescending(c => c.DateCreated).AsParallel().ToList();
            results.AddRange(_mapper.Map<List<Product>, List<ProductTitleViewModel>>(products.ToList()));
            return results;
        }

        public PagingViewModel SearchProducts(int categoryId, string name, bool asTitle, int pageSize, int pageIndex)
        {
            var pagingResult = new PagingViewModel();
            var allProducts = new List<Product>();

            if (categoryId != 0)
            {
                var category = _dbContext.Categories.Find(categoryId);

                if (category.IsDeleted || category == null) return pagingResult;
                if (name.IsNullOrEmpty())
                {
                    allProducts = category.ProductCategories
                    .Where(pc => !pc.IsDeleted && !pc.Product.IsDeleted)
                    .Select(pc => pc.Product)
                    .OrderByDescending(pc => pc.DateCreated)
                    .AsParallel().ToList();
                }
                else
                {
                    allProducts = category.ProductCategories
                    .Where(pc => !pc.IsDeleted && !pc.Product.IsDeleted)
                    .Select(pc => pc.Product)
                    .Where(p => EF.Functions.FreeText(p.Name, name))
                    .OrderByDescending(pc => pc.DateCreated)
                    .AsParallel().ToList();
                }

            }
            else
            {
                if (name.IsNullOrEmpty())
                {
                    allProducts = _dbContext.Products
                                    .Where(p => !p.IsDeleted)
                                    .OrderByDescending(p => p.DateCreated)
                                    .AsParallel()
                                    .ToList();
                }
                else
                {
                    allProducts = _dbContext.Products
                                    .Where(p => !p.IsDeleted && EF.Functions.FreeText(p.Name, name))
                                    .OrderByDescending(p => p.DateCreated)
                                    .AsParallel()
                                    .ToList();
                }
            }

            pagingResult.Total = allProducts.Count;
            pagingResult.TotalPages = (int)Math.Ceiling((double)pagingResult.Total / pageSize);
            var products = allProducts.Skip(pageSize * pageIndex).Take(pageSize).AsParallel().ToList();

            if (asTitle)
            {
                pagingResult.Results = _mapper.Map<List<Product>, List<ProductTitleViewModel>>(products);
            }
            else
            {
                pagingResult.Results = _mapper.Map<List<Product>, List<ProductDetailViewModel>>(products);
            }

            return pagingResult;
        }
    }
}
