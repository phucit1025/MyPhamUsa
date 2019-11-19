using Castle.Core.Internal;
using Microsoft.AspNetCore.Identity;
using MyPhamUsa.Data;
using MyPhamUsa.Services.Interfaces;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyPhamUsa.Services.Implementations
{
    public class ToolService : IToolService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ToolService(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task CreateAdmin(string username, string password)
        {
            var newUser = new IdentityUser { UserName = username, Email = username };
            await _userManager.CreateAsync(newUser, password);
            await _userManager.AddToRoleAsync(newUser, "Admin");
        }

        public void DeleteCategoryMappings()
        {
            var deletedMappings = _context.ProductCategories.Where(p => !p.IsDeleted).ToList();
            if (deletedMappings.Any())
            {
                _context.RemoveRange(deletedMappings);
                _context.SaveChanges();
            }
        }

        public void DeleteImages()
        {
            var deletedImages = _context.Images.Where(i => i.IsDeleted).ToList();
            if (deletedImages.Any())
            {
                foreach (var img in deletedImages)
                {
                    if (!img.Path.IsNullOrEmpty())
                    {
                        File.Delete(img.Path);
                    }
                    _context.Remove(img);
                }
                _context.SaveChanges();
            }
        }
    }
}
