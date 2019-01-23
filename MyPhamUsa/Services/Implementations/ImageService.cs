using System;
using System.Collections.Generic;
using MyPhamUsa.Models.ViewModels;
using MyPhamUsa.Services.Interfaces;
using MyPhamUsa.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using MoreLinq;
using System.Linq;
using AutoMapper;
using MyPhamUsa.Models.Entities;

namespace MyPhamUsa.Services.Implementations
{
    public class ImageService : IImageService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public ImageService(AppDbContext context, IHttpContextAccessor httpContext,IMapper mapper)
        {
            _context = context;
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public bool DeleteImage(int id)
        {
            var image = _context.Images.Find(id);
            _context.Images.Remove(image);
            try
            {
                var imagePath = Path.Combine($"wwwroot/images", image.Path.Split(@"/").Last());
                File.Delete(imagePath);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }

        }

        public ICollection<ImageViewModel> GetImages(int productId)
        {
            var images = _context.Images.Where(i => i.ProductId == productId && !i.IsDeleted).ToList();
            var results = _mapper.Map<List<Image>, List<ImageViewModel>>(images);
            return results;
        }
    }
}
