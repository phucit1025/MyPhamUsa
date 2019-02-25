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
    public class StorageService : IStorageService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public StorageService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public DailyReportViewModel GetDailyReport(DateTime date)
        {
            var issueTransactions = _context.Storages.Where(s => !s.IsDeleted && s.IsIssued && s.DateUpdated.Day == date.Day && s.DateUpdated.Month == date.Month).ToList();
            if (issueTransactions.Any())
            {
                var total = 0;
                foreach(var transaction in issueTransactions)
                {
                    total += (transaction.Quantity * Convert.ToInt32(transaction.Product.Price));
                }
                return new DailyReportViewModel()
                {
                    TotalPrice = total.ToString()
                };
            }

            return new DailyReportViewModel()
            {
                TotalPrice = "0"
            };
        }

        public StorageViewModel GetStorage(int id)
        {
            var storage = _context.Storages.Find(id);
            if (storage.IsDeleted)
            {
                return null;
            }
            return _mapper.Map<Storage, StorageViewModel>(storage);
        }

        public ICollection<StorageViewModel> GetStorages()
        {
            var storages = _context.Storages.Where(s => !s.IsDeleted && !s.Product.IsDeleted).OrderByDescending(s => s.DateCreated).ToList();
            var results = _mapper.Map<List<Storage>, List<StorageViewModel>>(storages);
            return results;
        }

        public ICollection<StorageViewModel> GetStorages(bool isIssued)
        {
            return GetStorages().Where(s => s.IsIssued == isIssued).ToList();
        }

        public ICollection<StorageViewModel> GetStorages(int productId)
        {
            var storages = _context.Storages.Where(s => s.ProductId == productId && !s.IsDeleted).OrderByDescending(s => s.DateCreated).ToList();
            var results = _mapper.Map<List<Storage>, List<StorageViewModel>>(storages);
            return results;
        }

        public ICollection<StorageViewModel> GetStorages(int productId, bool isIssued)
        {
            var storages = _context.Storages.Where(s => s.ProductId == productId && !s.IsDeleted && s.IsDeleted == isIssued).OrderByDescending(s => s.DateCreated).ToList();
            var results = _mapper.Map<List<Storage>, List<StorageViewModel>>(storages);
            return results;
        }

        public ICollection<StorageViewModel> GetStorages(int productId, bool isIssued, int orderId)
        {
            var storages = _context.Storages.Where(s => s.ProductId == productId && !s.IsDeleted && s.IsDeleted == isIssued && s.OrderId == orderId).OrderByDescending(s => s.DateCreated).ToList();
            var results = _mapper.Map<List<Storage>, List<StorageViewModel>>(storages);
            return results;
        }

        public ICollection<StorageViewModel> GetStorages(int? productId, int day, int month, int? year)
        {
            List<StorageViewModel> results = new List<StorageViewModel>();
            IQueryable<Storage> storages = null;
            if (productId.HasValue)
            {
                storages = _context.Storages.Where(c => c.ProductId == productId && !c.IsDeleted);
                storages = storages.Where(s => s.DateCreated.Day == day && s.DateCreated.Month == month);
            }
            else
            {
                storages = _context.Storages.Where(s => s.DateCreated.Day == day && s.DateCreated.Month == month && !s.IsDeleted);
            }
            if (year.HasValue)
            {
                storages = storages.Where(s => s.DateCreated.Year == year);
            }
            results = _mapper.Map<List<Storage>, List<StorageViewModel>>(storages.OrderByDescending(s => s.DateCreated).ToList());
            return results;
        }

        public bool Issue(IRViewModel issueModel)
        {
            var issue = _mapper.Map<IRViewModel, Storage>(issueModel);
            issue.IsIssued = true;
            try
            {
                var product = _context.Products.Find(issueModel.ProductId);
                if (product.QuantityIndex >= issueModel.Quantity)
                {
                    product.QuantityIndex -= issueModel.Quantity;
                    _context.Add(issue);
                    _context.Update(product);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (DbUpdateException)
            {
                return false;
            }

        }

        public bool Receive(IRViewModel receiveModel)
        {
            var receive = _mapper.Map<IRViewModel, Storage>(receiveModel);
            try
            {
                var product = _context.Products.Find(receiveModel.ProductId);
                product.QuantityIndex += receiveModel.Quantity;
                _context.Add(receive);
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
}
