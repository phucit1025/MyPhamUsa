using AutoMapper;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using MyPhamUsa.Data;
using MyPhamUsa.Models.Entities;
using MyPhamUsa.Models.ViewModels;
using MyPhamUsa.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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
                foreach (var transaction in issueTransactions)
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

        public FilterStorageViewModel GetStorages(StorageFilterViewModel filter)
        {
            var storageResults = new List<StorageViewModel>();
            var storages = new List<Storage>();

            if (filter.IsIssued.HasValue)
            {
                storages.AddRange(_context.Storages.Where(s => !s.IsDeleted && s.IsIssued == filter.IsIssued.Value));
            }
            if (filter.Time.HasValue)
            {
                if (storages.Any() && (!filter.NameOrCode.IsNullOrEmpty() || filter.IsIssued.HasValue))
                {
                    storages = storages.Where(s => s.DateCreated.ToShortDateString().Equals(filter.Time.Value.ToShortDateString())).ToList();
                }
                else if (filter.NameOrCode.IsNullOrEmpty() && !filter.IsIssued.HasValue)
                {
                    storages.AddRange(_context.Storages.Where(s => !s.IsDeleted && s.DateCreated.ToShortDateString().Equals(filter.Time.Value.ToShortDateString())));
                }
            }
            if (!filter.NameOrCode.IsNullOrEmpty())
            {
                if (storages.Any() && (filter.IsIssued.HasValue || filter.Time.HasValue))
                {
                    storages = storages.Where(s => s.Product.Name.Contains(filter.NameOrCode, StringComparison.CurrentCultureIgnoreCase) || s.Product.Code.Contains(filter.NameOrCode, StringComparison.CurrentCultureIgnoreCase)).ToList();
                }
                else if (!filter.IsIssued.HasValue && !filter.Time.HasValue)
                {
                    storages.AddRange(_context.Storages.Where(s => !s.IsDeleted && (s.Product.Name.Contains(filter.NameOrCode, StringComparison.CurrentCultureIgnoreCase) || s.Product.Code.Contains(filter.NameOrCode, StringComparison.CurrentCultureIgnoreCase))));
                }
            }

            if (filter.NameOrCode.IsNullOrEmpty() && !filter.IsIssued.HasValue && !filter.Time.HasValue)
            {
                storages = _context.Storages.Where(s => !s.IsDeleted).OrderByDescending(s => s.DateCreated).ToList();
            }

            if (storages.Any())
            {
                storageResults = _mapper.Map<List<Storage>, List<StorageViewModel>>(storages);
                var totalPrice = GetTotal(storages, false);
                var totalSellPrice = GetTotal(storages, true);
                return new FilterStorageViewModel()
                {
                    Storages = storageResults,
                    TotalPrice = totalPrice,
                    TotalSellPrice = totalSellPrice
                };
            }
            else
            {
                return new FilterStorageViewModel();
            }
        }

        public StoragePagingViewModel GetStorages(int pageSize, int pageIndex)
        {
            var result = new StoragePagingViewModel();
            var totalStorages = _context.Storages.Where(s => !s.IsDeleted && !s.Product.IsDeleted).OrderByDescending(s => s.DateCreated).ToList();
            result.TotalPages = totalStorages.Count();
            var storages = totalStorages.Skip(pageSize * pageIndex).Take(pageSize).ToList();
            result.Results = _mapper.Map<List<Storage>, List<StorageViewModel>>(storages);
            return result;
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

        private string GetTotal(ICollection<Storage> storages, bool isSellPrice)
        {
            var total = 0;
            if (isSellPrice)
            {
                foreach (var storage in storages)
                {
                    total += (Convert.ToInt32(storage.Product.SellPrice) * storage.Quantity);
                }
                return total.ToString();
            }
            else
            {
                foreach (var storage in storages)
                {
                    total += (Convert.ToInt32(storage.Product.Price) * storage.Quantity);
                }
                return total.ToString();
            }

        }
    }
}
