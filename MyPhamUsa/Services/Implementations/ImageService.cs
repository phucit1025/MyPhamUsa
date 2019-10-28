using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using MyPhamUsa.Data;
using MyPhamUsa.Models.Entities;
using MyPhamUsa.Models.ViewModels;
using MyPhamUsa.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyPhamUsa.Services.Implementations
{
    public class ImageService : IImageService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public ImageService(AppDbContext context, IHttpContextAccessor httpContext, IMapper mapper)
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

        public bool UpdateImage(ImageUpdateViewModel updateModel)
        {
            try
            {
                if (updateModel.DeleteImages.Count > 0)
                {
                    foreach (var deleteId in updateModel.DeleteImages)
                    {
                        DeleteImage(deleteId);
                    }
                }
                if (updateModel.Base64Images.Count > 0)
                {
                    foreach (var base64 in updateModel.Base64Images)
                    {
                        string path = SaveImage(base64);
                        _context.Images.Add(new Image()
                        {
                            ProductId = updateModel.ProductId,
                            Path = path,
                        });
                    }
                }
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }


        }

        private string SaveImage(string base64)
        {
            base64 = base64.Split(",").ElementAt(1);
            string fileName;
            string imagePath;
            base64 = base64.Split(",").ElementAt(1);
            var request = _httpContext.HttpContext.Request;
            var url = $"{request.Scheme}://{request.Host}/";
            try
            {
                var base64array = Convert.FromBase64String(base64);
                fileName = Guid.NewGuid().ToString() + ".jpg";
                imagePath = Path.Combine($"wwwroot/images", fileName);
                File.WriteAllBytes(imagePath, base64array);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return $"{url}images/{fileName}";
        }
    }
}
