using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using MyPhamUsa.Services.Interfaces;
using MyPhamUsa.Models.ViewModels;

namespace MyPhamUsa.Controllers
{
    [Route("api/Storage/[action]")]
    public class StorageController : Controller
    {
        private readonly IStorageService _storageService;

        public StorageController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpPost]
        public IActionResult Issue([FromBody] IRViewModel model)
        {
            if (_storageService.Issue(model))
            {
                return StatusCode(200);
            }
            else
            {
                return StatusCode(400);
            }
        }

        [HttpPost]
        public IActionResult Receive([FromBody] IRViewModel model)
        {
            if (_storageService.Receive(model))
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
