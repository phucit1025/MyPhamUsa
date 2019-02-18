using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using MyPhamUsa.Services.Interfaces;
using MyPhamUsa.Models.ViewModels;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using MyPhamUsa.Extensions;

namespace MyPhamUsa.Controllers
{
    [Route("api/Account/[action]")]
    [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            var token = await _accountService.Login(loginViewModel);
            if (token.IsNullOrEmpty())
            {
                return StatusCode(400);
            }
            else
            {
                return StatusCode(200, token);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginV2([FromBody] LoginViewModel loginViewModel)
        {
            var resultToken = await _accountService.Login(loginViewModel);
            if (resultToken.IsNullOrEmpty())
            {
                return StatusCode(400);
            }
            else
            {
                return StatusCode(200, new {token = resultToken });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAdminInformation()
        {
            var username = await _accountService.GetUserInfo();
            return StatusCode(200, username);
        }

        [HttpPost]
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
    }
}
