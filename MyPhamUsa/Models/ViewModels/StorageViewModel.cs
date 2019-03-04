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

    public class IRViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
    }

    public class DailyReportViewModel
    {
        public string TotalPrice { get; set; }
    }
}
