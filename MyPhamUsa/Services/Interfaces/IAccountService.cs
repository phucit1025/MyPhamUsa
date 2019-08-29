using MyPhamUsa.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyPhamUsa.Services.Interfaces
{
    public interface IAccountService
    {
        Task<string> Login(LoginViewModel loginViewModel);
        Task<object> LoginV2(LoginViewModel loginViewModel);
        Task<String> GetUserInfo();
        Task<ICollection<AccountViewModel>> GetAccounts();
        Task<string> CreateUser(LoginViewModel loginViewModel);
        Task<bool> RemoveUser(string guid);
        Task<bool> ChangePassword(ChangePasswordViewModel changePasswordViewModel);
    }
}
