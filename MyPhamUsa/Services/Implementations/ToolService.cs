using Castle.Core.Internal;
using MyPhamUsa.Data;
using MyPhamUsa.Services.Interfaces;
using System.IO;
using System.Linq;

namespace MyPhamUsa.Services.Implementations
{
    public class ToolService : IToolService
    {
        private readonly AppDbContext _context;

        public ToolService(AppDbContext context)
        {
            _context = context;
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
