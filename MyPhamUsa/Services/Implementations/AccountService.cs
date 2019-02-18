using System;
using MyPhamUsa.Models.ViewModels;
using MyPhamUsa.Services.Interfaces;
using MyPhamUsa.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using MyPhamUsa.Extensions;
using AutoMapper;

namespace MyPhamUsa.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AccountService(
                            AppDbContext context,
                            IHttpContextAccessor httpContextAccessor,
                            UserManager<IdentityUser> userManager,
                            RoleManager<IdentityRole> roleManager,
                            IConfiguration configuration,
                            IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<bool> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            var guid = _httpContextAccessor.HttpContext.User.GetGuid();
            var user = await _userManager.FindByIdAsync(guid);
            if (await _userManager.CheckPasswordAsync(user, changePasswordViewModel.OldPassword))
            {
                await _userManager.ChangePasswordAsync(user, changePasswordViewModel.OldPassword, changePasswordViewModel.NewPassword);
                return true;
            }
            return false;
        }
        public async Task<bool> RemoveUser(string guid)
        {
            var user = _context.Users.Find(guid);
            if (user != null)
            {
                var isSuccess = await _userManager.DeleteAsync(user);
                return isSuccess.Succeeded;
            }
            return false;
        }

        public async Task<string> CreateUser(LoginViewModel loginViewModel)
        {
            var user = await _userManager.FindByNameAsync(loginViewModel.Username);
            if (user != null)
            {
                var newUser = new IdentityUser() { Email = loginViewModel.Username, UserName = loginViewModel.Username };
                var result = await _userManager.CreateAsync(newUser, loginViewModel.Password);
                if (result.Succeeded)
                {
                    return newUser.Id;
                }
            }
            return "";
        }

        public async Task<ICollection<AccountViewModel>> GetAccounts()
        {
            var currentGuid = _httpContextAccessor.HttpContext.User.GetGuid();
            var users = await _userManager.GetUsersInRoleAsync("Admin");
            var results = _mapper.Map<List<IdentityUser>, List<AccountViewModel>>(users.Where(u => !u.Id.Equals(currentGuid)).ToList());
            return results;
        }

        public async Task<string> GetUserInfo()
        {
            var guid = _httpContextAccessor.HttpContext.User.GetGuid();
            var user = await _userManager.FindByIdAsync(guid);
            return user.UserName;
        }

        public async Task<string> Login(LoginViewModel loginViewModel)
        {
            var user = await _userManager.FindByEmailAsync(loginViewModel.Username);
            if (user != null)
            {
                var hasRightPassword = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

                if (hasRightPassword)
                {
                    var roleList = await _userManager.GetRolesAsync(user);
                    var userRole = roleList.FirstOrDefault();

                    #region Set Claims
                    var claims = new List<Claim>() {
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim(new ClaimsIdentityOptions().UserIdClaimType, user.Id),
                    };
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                    claims.Add(new Claim(new ClaimsIdentityOptions().SecurityStampClaimType, await _userManager.GetSecurityStampAsync(user)));
                    #endregion

                    return BuildJwtToken(claims);
                }
            }

            return null;
        }



        private string BuildJwtToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(0),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
