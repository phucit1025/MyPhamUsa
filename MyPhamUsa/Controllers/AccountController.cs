using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPhamUsa.Extensions;
using MyPhamUsa.Models.ViewModels;
using MyPhamUsa.Services.Interfaces;
using System.Threading.Tasks;

namespace MyPhamUsa.Controllers
{
    [Route("api/Account/[action]")]
    [Authorize(Roles = "Admin, Staff")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginV2([FromBody] LoginViewModel loginViewModel)
        {
            var result = await _accountService.LoginV2(loginViewModel);
            if (result == null)
            {
                return StatusCode(400);
            }
            else
            {
                return StatusCode(200, result);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAdminInformation()
        {
            var username = await _accountService.GetUserInfo();
            return StatusCode(200, username);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody]LoginViewModel newAccount)
        {
            var newGuid = await _accountService.CreateUser(newAccount);
            if (newGuid.IsNullOrEmpty())
            {
                return StatusCode(400);
            }
            else
            {
                return StatusCode(200, newGuid);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var results = await _accountService.GetAccounts();
            return StatusCode(200, results);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordViewModel changePasswordViewModel)
        {
            if (await _accountService.ChangePassword(changePasswordViewModel)) return StatusCode(200);
            return StatusCode(400);
        }

        [HttpGet]
        public IActionResult TestAuth()
        {
            var guid = User.GetGuid();
            return StatusCode(200, guid);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (await _accountService.RemoveUser(id))
            {
                return StatusCode(200);
            }
            else
            {
                return StatusCode(400);
            }
        }

    }
}
