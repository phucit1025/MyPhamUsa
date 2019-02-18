using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPhamUsa.Models.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using MyPhamUsa.Services.Interfaces;

namespace MyPhamUsa.Controllers
{
    [Route("api/Image/[action]")]
    [Authorize(Roles ="Admin")]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet]
        public IActionResult GetImages(int productId)
        {
            var results = _imageService.GetImages(productId);
            return StatusCode(200, results);
        }

        [HttpDelete]
        public IActionResult DeleteImage(int id)
        {
            if (_imageService.DeleteImage(id))
            {
                return StatusCode(200);
            }
            else
            {
                return StatusCode(400);
            }


        }

        [HttpPost]
        public IActionResult UpdateImages([FromBody] ImageUpdateViewModel updateModel)
        {
            if (_imageService.UpdateImage(updateModel))
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
