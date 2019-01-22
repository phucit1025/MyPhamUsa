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
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool CreateProduct(ProductCreateViewModel newProduct)
        {
            try
            {
                var product = _mapper.Map<ProductCreateViewModel, Product>(newProduct);
                _context.Add(product);
                _context.SaveChanges();
                var storageReceive = new Storage()
                {
                    Quantity = newProduct.ReceiveQuantity,
                    ProductId = product.Id,
                    Description = "Nhập kho sản phẩm mới."
                };
                _context.Add(storageReceive);
                _context.SaveChanges();
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
    }
}
