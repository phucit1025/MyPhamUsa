namespace MyPhamUsa.Models.ViewModels
{
    public class StatisticsViewModel
    {
        public string Price { get; set; } = "0";
        public string SellPrice { get; set; } = "0";
    }

    public class CurrentStorageReport
    {
        public string TotalSellPrice { get; set; } = "0";
        public string TotalPrice { get; set; } = "0";
        public string TotalQuantity { get; set; } = "0";
    }
}
