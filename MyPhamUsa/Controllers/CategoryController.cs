using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using MyPhamUsa.Services.Interfaces;
using MyPhamUsa.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

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
