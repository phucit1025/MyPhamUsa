namespace MyPhamUsa.Models.ViewModels
{
    public class AccountViewModel
    {
        public string Guid { get; set; }
        public string Username { get; set; }
    }

    public class ChangePasswordViewModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
