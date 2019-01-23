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
using System.IO;
using Microsoft.AspNetCore.Http;

namespace MyPhamUsa.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private IHttpContextAccessor _httpContext;

        public ProductService(AppDbContext context, IMapper mapper,IHttpContextAccessor httpContext)
        {
            _context = context;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        public bool CreateProduct(ProductCreateViewModel newProduct)
        {
            try
            {
                #region Create Product
                var product = _mapper.Map<ProductCreateViewModel, Product>(newProduct);
                _context.Add(product);
                _context.SaveChanges();
                #endregion

                #region Image Processing
                foreach(var base64 in newProduct.Base64Images)
                {
                    string path = SaveImage(base64);
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

                return true;
            }
            catch (DbUpdateException)
            {
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

        public ICollection<ProductViewModel> GetProducts()
        {
            var products = _context.Products.Where(p => !p.IsDeleted).ToList();
            var results = _mapper.Map<List<Product>, List<ProductViewModel>>(products);
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

        public bool UpdateProduct(ProductViewModel newProduct)
        {
            var product = _context.Products.Find(newProduct.Id);
            var result = _mapper.Map<ProductViewModel, Product>(newProduct, product);
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

    }
}
