using AutoMapper;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
                    Description = "Nhập kho sản phẩm mới."
                };
                _context.Add(storageReceive);
                _context.SaveChanges();
                #endregion

                //#region Add To Category
                //foreach(var categoryId in newProduct.CategoryIds)
                //{
                //    _context.ProductCategories.Add(new ProductCategory()
                //    {
                //        CategoryId = categoryId,
                //        ProductId = product.Id
                //    });
                //}
                //_context.SaveChanges();
                //#endregion

                transaction.Commit();

                return true;
            }
            catch (DbUpdateException)
            {
                transaction.Rollback();
                if (path.IsNullOrEmpty())
                {
                    File.Delete(path);
                }
                return false;
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

        public ICollection<ClientProductViewModel> GetClientProducts()
        {
            var products = _context.Products.Where(p => !p.IsDeleted).OrderByDescending(p => p.DateCreated).ToList();
            var results = _mapper.Map<List<Product>, List<ClientProductViewModel>>(products);
            return results;
        }

        public ICollection<ProductViewModel> GetProducts()
        {
            var products = _context.Products.Where(p => !p.IsDeleted).ToList();
            var results = _mapper.Map<List<Product>, List<ProductViewModel>>(products);
            return results;
        }

        public ICollection<ProductOfStaffViewModel> GetProductsByStaff()
        {
            var products = _context.Products.Where(p => !p.IsDeleted).ToList();
            var results = _mapper.Map<List<Product>, List<ProductOfStaffViewModel>>(products);
            return results;
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
                var productCategories = product.ProductCategories;
                //Check Has New Category or Not
                foreach (var categoryId in newProduct.CategoryIds)
                {
                    if (!productCategories.Any(c => c.Id == categoryId))
                    {
                        _context.ProductCategories.Add(new ProductCategory()
                        {
                            ProductId = product.Id,
                            CategoryId = categoryId
                        });
                    }
                }
                //Check Has Deleted Category or Not
                foreach (var categoryMapping in productCategories)
                {
                    if (!newProduct.CategoryIds.Any(c => c == categoryMapping.CategoryId))
                    {
                        _context.ProductCategories.Remove(categoryMapping);
                    }
                }
                _context.SaveChanges();
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

        public ClientProductViewModel GetClientProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                var result = _mapper.Map<Product, ClientProductViewModel>(product);
                return result;
            }
            return null;
        }

        private string SaveImage(string base64)
        {
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

        public ICollection<ProductViewModel> GetProducts(int categoryId)
        {
            var category = _context.Categories.Find(categoryId);
            if (category.ProductCategories.Count != 0)
            {
                return _mapper.Map<List<Product>, List<ProductViewModel>>(category.ProductCategories.Select(c => c.Product).Where(p => !p.IsDeleted).ToList());
            }
            return new List<ProductViewModel>();
        }

        public ICollection<ClientProductViewModel> GetClientProducts(int categoryId)
        {
            var category = _context.Categories.Find(categoryId);
            if (category.ProductCategories.Count != 0)
            {
                return _mapper.Map<List<Product>, List<ClientProductViewModel>>(category.ProductCategories.Select(c => c.Product).Where(p => !p.IsDeleted).ToList());
            }
            return new List<ClientProductViewModel>();
        }

        public ICollection<ClientProductViewModel> GetClientProducts(List<int> productIds)
        {
            var results = new List<ClientProductViewModel>();
            productIds.ForEach(p =>
            {
                var product = _context.Products.Find(p);
                results.Add(_mapper.Map<Product, ClientProductViewModel>(product));
            });
            return results;
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

        public ICollection<ClientProductViewModel> SearchClientProducts(string name)
        {
            var products = _context.Products.Where(p => !p.IsDeleted && p.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase)).ToList();
            if (products.Any())
            {
                var results = _mapper.Map<List<Product>, List<ClientProductViewModel>>(products);
                return results;
            }
            else
            {
                return new List<ClientProductViewModel>();
            }

        }

        public ProductPagingViewModel GetProducts(int pageSize, int pageIndex)
        {
            var result = new ProductPagingViewModel();
            var totalProducts = _context.Products.Where(p => !p.IsDeleted).OrderByDescending(p=>p.DateCreated);
            result.TotalPages = totalProducts.Count();
            var products = totalProducts.Skip(pageSize * pageIndex).Take(pageSize).ToList();
            result.Results = _mapper.Map<List<Product>, List<ProductViewModel>>(products);
            return result;
        }

        public ProductPagingViewModel GetProducts(int categoryId, int pageSize, int pageIndex)
        {
            var result = new ProductPagingViewModel();
            var totalProducts = _context.ProductCategories.Where(c=>!c.IsDeleted && c.CategoryId == categoryId && !c.Product.IsDeleted).Select(p=>p.Product).OrderByDescending(p => p.DateCreated);
            result.TotalPages = totalProducts.Count();
            var products = totalProducts.Skip(pageSize * pageIndex).Take(pageSize).ToList();
            result.Results = _mapper.Map<List<Product>, List<ProductViewModel>>(products);
            return result;
        }

    }
}
