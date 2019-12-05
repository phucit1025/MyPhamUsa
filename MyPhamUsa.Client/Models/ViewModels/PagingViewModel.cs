namespace MyPhamUsa.Client.Models.ViewModels
{
    public class PagingViewModel
    {
        public int Total { get; set; } = 0;
        public int TotalPages { get; set; } = 1;
        public dynamic Results { get; set; }
    }
}
