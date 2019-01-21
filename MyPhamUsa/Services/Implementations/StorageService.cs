using AutoMapper;
using MyPhamUsa.Data;
using MyPhamUsa.Models.ViewModels;
using MyPhamUsa.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
        }

        public ICollection<StorageViewModel> GetStorages(bool isIssued)
        {
            throw new NotImplementedException();
        }

        public ICollection<StorageViewModel> GetStorages(int productId)
        {
            throw new NotImplementedException();
        }

        public ICollection<StorageViewModel> GetStorages(int productId, bool isIssued)
        {
            throw new NotImplementedException();
        }

        public ICollection<StorageViewModel> GetStorages(int productId, bool isIssued, int orderId)
        {
            throw new NotImplementedException();
        }

        public bool Issue(int productId, int quantity, string description = "")
        {
            throw new NotImplementedException();
        }

        public bool Receive(int productId, int quantity, string description = "")
        {
            throw new NotImplementedException();
        }
    }
}
