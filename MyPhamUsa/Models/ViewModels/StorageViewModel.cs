using System;
using System.Collections;
using System.Collections.Generic;

namespace MyPhamUsa.Models.ViewModels
{
    public class StorageViewModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public bool IsIssued { get; set; }
        public ProductViewModel Product { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }

    public class FilterStorageViewModel
    {
        public List<StorageViewModel> Storages { get; set; } = new List<StorageViewModel>();
        public string TotalPrice { get; set; } = "0";
        public string TotalSellPrice { get; set; } = "0";
    }

    public class IRViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
    }

    public class IssueToOrderViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
    }

    public class DailyReportViewModel
    {
        public string TotalPrice { get; set; }
    }

    public class StorageFilterViewModel
    {
        public DateTime? Time { get; set; }
        public bool? IsIssued { get; set; }
        public string NameOrCode { get; set; }
    }

    public class StoragePagingViewModel
    {
        public int TotalPages { get; set; }
        public ICollection<StorageViewModel> Results { get; set; } = new List<StorageViewModel>();
    }
}
