using AutoMapper;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using MyPhamUsa.Data;
using MyPhamUsa.Models.Entities;
using MyPhamUsa.Models.ViewModels;
using MyPhamUsa.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyPhamUsa.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private IHttpContextAccessor _httpContext;

        public ProductService(AppDbContext context, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _context = context;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        public bool CreateProduct(ProductCreateViewModel newProduct)
        {
            var transaction = _context.Database.BeginTransaction();
            string path = "";
            try
            {

                #region Create Product
                var product = _mapper.Map<ProductCreateViewModel, Product>(newProduct);
                _context.Add(product);
                _context.SaveChanges();
                #endregion

                #region Image Processing
                foreach (var base64 in newProduct.Base64Images)
                {
                    path = SaveImage(base64);
                    _context.Images.Add(new Image()
                    {
                        ProductId = product.Id,
                        Path = path,
                    });
                }
                _context.SaveChanges();
                #endregion

                #region Storage Processing
                var storageReceive = new Storage()
                {
                    Quantity = newProduct.ReceiveQuantity,
                    ProductId = product.Id,
                    Description = "Nhập hàng sản phẩm mới."
                };
                _context.Add(storageReceive);
                _context.SaveChanges();
                #endregion

                #region Category
                var categories = newProduct.CategoryIds.Select(catId => new ProductCategory()
                {
                    CategoryId = catId,
                    ProductId = product.Id
                }).ToList();
                _context.AddRange(categories);
                _context.SaveChanges();
                #endregion

                transaction.Commit();

                return true;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                if (path.IsNullOrEmpty())
                {
                    File.Delete(path);
                }
                return false;
            }
            finally
            {
                transaction.Dispose();
            }

        }

        public bool DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                product.IsDeleted = true;
                try
                {
                    _context.Update(product);
                    _context.SaveChanges();
                    return true;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
            }
            return false;
        }

        public ProductOfStaffPagingViewModel GetProductsByStaff(int pageSize, int pageIndex)
        {
            var result = new ProductOfStaffPagingViewModel();
            var totalProducts = _context.Products.Where(p => !p.IsDeleted).OrderByDescending(p => p.DateCreated);
            result.Total = totalProducts.Count();
            result.TotalPages = (int)Math.Ceiling((double)totalProducts.Count() / pageSize);
            var products = totalProducts.Skip(pageSize * pageIndex).Take(pageSize).ToList();
            result.Results = _mapper.Map<List<Product>, List<ProductOfStaffViewModel>>(products);
            return result;
        }

        public bool RenewQuantityIndex(int productId)
        {
            var receiveQuantity = _context.Storages.Where(s => s.ProductId == productId && !s.IsDeleted && !s.IsIssued).Sum(c => c.Quantity);
            var issueQuantity = _context.Storages.Where(s => s.ProductId == productId && !s.IsDeleted && s.IsIssued).Sum(c => c.Quantity);
            var availableQuantity = receiveQuantity - issueQuantity;
            if (availableQuantity < 0)
            {
                return false;
            }
            else
            {
                var product = _context.Products.Find(productId);
                product.QuantityIndex = availableQuantity;
                try
                {
                    _context.Update(product);
                    _context.SaveChanges();
                    return true;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
            }
        }

        public bool UpdateProduct(ProductUpdateViewModel newProduct)
        {
            var product = _context.Products.Find(newProduct.Id);
            var result = _mapper.Map<ProductUpdateViewModel, Product>(newProduct, product);
            var tracker = _context.Database.BeginTransaction();
            try
            {
                #region Update Info
                _context.Update(result);
                _context.SaveChanges();
                #endregion

                #region Update Categories
                var currentCategories = product.ProductCategories.Where(c => !c.IsDeleted).ToList();
                var newCategories = newProduct.CategoryIds.Select(c => new ProductCategory() { CategoryId = c }).ToList();

                var deletedCategories = currentCategories.ExceptBy(newCategories, c => c.CategoryId).ToList();
                if (deletedCategories.Any())
                {
                    foreach (var deletedCategory in deletedCategories)
                    {
                        deletedCategory.IsDeleted = true;
                        deletedCategory.DateUpdated = DateTime.Now;
                        _context.Update(deletedCategory);
                    }
                    _context.SaveChanges();
                }

                var addedCategories = newCategories.ExceptBy(currentCategories, c => c.CategoryId).ToList();
                if (addedCategories.Any())
                {
                    _context.AddRange(addedCategories.Select(c => new ProductCategory() { CategoryId = c.CategoryId, ProductId = newProduct.Id }));
                    _context.SaveChanges();
                }
                #endregion

                tracker.Commit();
                return true;
            }
            catch (DbUpdateException)
            {
                tracker.Rollback();
                return false;
            }
        }

        private string SaveImage(string base64)
        {
            base64 = base64.Split(",").ElementAt(1);
            string fileName;
            string imagePath;
            var request = _httpContext.HttpContext.Request;
            var url = $"{request.Scheme}://{request.Host}/";
            try
            {
                var base64array = Convert.FromBase64String(base64);
                fileName = Guid.NewGuid().ToString() + ".jpg";
                imagePath = Path.Combine($"wwwroot/images", fileName);
                File.WriteAllBytes(imagePath, base64array);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return $"{url}images/{fileName}";
        }

        public bool UpdateProduct(ProductStaffUpdateViewModel newProduct)
        {
            var oldProduct = _context.Products.Find(newProduct.Id);
            var product = _mapper.Map<ProductStaffUpdateViewModel, Product>(newProduct, oldProduct);
            try
            {
                _context.Update(product);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public ProductViewModel GetProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                var result = _mapper.Map<Product, ProductViewModel>(product);
                return result;
            }
            return null;
        }

        public bool IsAvailableCode(ProductCodeValidViewModel model)
        {
            var existedCodes = new List<Product>();
            if (model.ProductId != 0)
            {
                existedCodes = _context.Products.Where(c => c.Code.Equals(model.Code.Trim(), StringComparison.CurrentCultureIgnoreCase) && c.Id != model.ProductId).ToList();
            }
            else
            {
                existedCodes = _context.Products.Where(c => c.Code.Equals(model.Code.Trim(), StringComparison.CurrentCultureIgnoreCase)).ToList();

            }

            if (existedCodes.Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ProductPagingViewModel GetProducts(int pageSize, int pageIndex)
        {

            var result = new ProductPagingViewModel();
            var totalProducts = _context.Products.Where(p => !p.IsDeleted).OrderByDescending(p => p.DateCreated);
            result.Total = totalProducts.Count();
            result.TotalPages = (int)Math.Ceiling((double)totalProducts.Count() / pageSize);
            var products = totalProducts.Skip(pageSize * pageIndex).Take(pageSize).ToList();
            result.Results = _mapper.Map<List<Product>, List<ProductViewModel>>(products);
            return result;
        }

        public ProductPagingViewModel GetProducts(int categoryId, int pageSize, int pageIndex)
        {
            var result = new ProductPagingViewModel();
            var totalProducts = _context.ProductCategories.Where(c => !c.IsDeleted && c.CategoryId == categoryId && !c.Product.IsDeleted).Select(p => p.Product).OrderByDescending(p => p.DateCreated);
            result.Total = totalProducts.Count();
            result.TotalPages = (int)Math.Ceiling((double)totalProducts.Count() / pageSize);
            var products = totalProducts.Skip(pageSize * pageIndex).Take(pageSize).ToList();
            result.Results = _mapper.Map<List<Product>, List<ProductViewModel>>(products);
            return result;
        }

        public ProductPagingViewModel SearchProducts(string name, string code, int pageSize, int pageIndex)
        {
            var result = new ProductPagingViewModel();
            var allProducts = new List<Product>();
            if (code.IsNullOrEmpty()) { code = ""; }
            if (!name.IsNullOrEmpty())
            {
                allProducts = _context.Products.Where(q => !q.IsDeleted && EF.Functions.FreeText(q.Name, name)).ToList().Where(p => p.Code.Contains(code, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
            else
            {
                allProducts = _context.Products.Where(q => !q.IsDeleted).OrderByDescending(q => q.DateCreated).ToList().Where(p => p.Code.Contains(code, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
            if (allProducts.Any())
            {
                result.Total = allProducts.Count();
                result.TotalPages = (int)Math.Ceiling((double)result.Total / pageSize);
                result.Results = _mapper.Map<List<Product>, List<ProductViewModel>>(allProducts.Skip(pageSize * pageIndex).Take(pageSize).ToList());
            }
            return result;
        }

    }
}
