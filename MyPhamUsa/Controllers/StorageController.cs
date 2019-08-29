using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPhamUsa.Models.ViewModels;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using MyPhamUsa.Services.Interfaces;
using System;
using System.Linq;

namespace MyPhamUsa.Controllers
{
    [Route("api/Storage/[action]")]
    [Authorize(Roles = "Admin, Staff")]
    [ApiController]
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
        [Authorize(Roles = "Admin")]
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

        [HttpGet]
        public IActionResult GetStorages()
        {
            var results = _storageService.GetStorages();
            return StatusCode(200, results);
        }

        [HttpGet]
        public IActionResult GetStoragesPaging(int pageSize = 20, int pageIndex = 0)
        {
            var result = _storageService.GetStorages(pageSize,pageIndex);
            return StatusCode(200, new { totalPages = result.TotalPages, results = result.Results});
        }

        [HttpGet]
        public IActionResult GetStorage(int id)
        {
            var result = _storageService.GetStorage(id);
            return StatusCode(200, result);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult FilterStorages(DateTime? time, bool? isIssued, string nameOrCode, int pageSize, int pageIndex = 1)
        {
            var result = _storageService.GetStorages(new StorageFilterViewModel() { Time = time, IsIssued = isIssued, NameOrCode = nameOrCode });
            var totalRecord = result.Storages.Count;
            var totalPage = Math.Ceiling((double)totalRecord / (double)pageSize);
            #region Paging
            result.Storages = result.Storages.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            #endregion
            return StatusCode(200, new { storages = result.Storages, totalPage, pageIndex, total = totalRecord, totalPrice = result.TotalPrice, totalSellPrice = result.TotalSellPrice });
        }

        [HttpPost]
        public IActionResult GetDailyReport([FromBody] DateTime date)
        {
            var result = _storageService.GetDailyReport(date);
            return StatusCode(200, result);
        }
    }
}
