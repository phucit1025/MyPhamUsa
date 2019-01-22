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

        public ICollection<StorageViewModel> GetStorages()
        {
            var storages = _context.Storages.Where(s => !s.IsDeleted).ToList();
            var results = _mapper.Map<List<Storage>, List<StorageViewModel>>(storages);
            return results;
        }

        public ICollection<StorageViewModel> GetStorages(bool isIssued)
        {
            return GetStorages().Where(s=>s.IsIssued==isIssued).ToList();
        }

        public ICollection<StorageViewModel> GetStorages(int productId)
        {
            var storages = _context.Storages.Where(s => s.ProductId == productId && !s.IsDeleted).ToList();
            var results = _mapper.Map<List<Storage>, List<StorageViewModel>>(storages);
            return results;
        }

        public ICollection<StorageViewModel> GetStorages(int productId, bool isIssued)
        {
            var storages = _context.Storages.Where(s => s.ProductId == productId && !s.IsDeleted && s.IsDeleted==isIssued).ToList();
            var results = _mapper.Map<List<Storage>, List<StorageViewModel>>(storages);
            return results;
        }

        public ICollection<StorageViewModel> GetStorages(int productId, bool isIssued, int orderId)
        {
            var storages = _context.Storages.Where(s => s.ProductId == productId && !s.IsDeleted && s.IsDeleted == isIssued && s.OrderId==orderId).ToList();
            var results = _mapper.Map<List<Storage>, List<StorageViewModel>>(storages);
            return results;
        }

        public bool Issue(IRViewModel issueModel)
        {
            var issue = _mapper.Map<IRViewModel, Storage>(issueModel);
            issue.IsIssued = true;
            try
            {
                _context.Add(issue);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }

        }

        public bool Receive(IRViewModel receiveModel)
        {
            var issue = _mapper.Map<IRViewModel, Storage>(receiveModel);
            try
            {
                _context.Add(issue);
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
