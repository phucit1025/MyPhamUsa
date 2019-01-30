using MyPhamUsa.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPhamUsa.Services.Interfaces
{
    public interface IStorageService
    {
        ICollection<StorageViewModel> GetStorages();
        ICollection<StorageViewModel> GetStorages(bool isIssued);
        ICollection<StorageViewModel> GetStorages(int productId);
        ICollection<StorageViewModel> GetStorages(int productId, bool isIssued);
        ICollection<StorageViewModel> GetStorages(int productId, bool isIssued, int orderId);
        ICollection<StorageViewModel> GetStorages(int? productId, int day, int month, int? year);
        StorageViewModel GetStorage(int id);
        bool Issue(IRViewModel issueModel);
        bool Receive(IRViewModel receiveModel);

    }
}
