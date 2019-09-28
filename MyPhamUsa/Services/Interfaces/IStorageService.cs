using MyPhamUsa.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace MyPhamUsa.Services.Interfaces
{
    public interface IStorageService
    {
        ICollection<StorageViewModel> GetStorages();
        StoragePagingViewModel GetStorages(int pageSize, int pageIndex);
        FilterStorageViewModel GetStorages(StorageFilterViewModel filter);
        StorageViewModel GetStorage(int id);
        bool Issue(IRViewModel issueModel);
        bool Receive(IRViewModel receiveModel);
        
    }
}
