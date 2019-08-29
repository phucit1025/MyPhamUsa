using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPhamUsa.Models.ViewModels;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using MyPhamUsa.Services.Interfaces;

namespace MyPhamUsa.Controllers
{
    [Route("api/Category/[action]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetCategories()
        {
            var results = _categoryService.GetCategories();
            return StatusCode(200, results);
        }

        [HttpPost]
        [Authorize(Roles ="Admin, Staff")]
        public IActionResult CreateCategory([FromBody] CategoryCreateViewModel newCategory)
        {
            if (_categoryService.CreateCategory(newCategory)) return StatusCode(200);
            return StatusCode(400);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Staff")]
        public IActionResult UpdateCategory([FromBody] CategoryViewModel newCategory)
        {
            if (_categoryService.UpdateCategory(newCategory)) return StatusCode(200);
            return StatusCode(400);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, Staff")]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (_categoryService.DeleteCategory(categoryId)) return StatusCode(200);
            return StatusCode(400);
        }
    }
}
