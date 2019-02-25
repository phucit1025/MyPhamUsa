﻿using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPhamUsa.Models.ViewModels;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using MyPhamUsa.Services.Interfaces;

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
        [Route("Product/{productId}")]
        public IActionResult GetStorages(int productId)
        {
            var results = _storageService.GetStorages(productId);
            return StatusCode(200, results);
        }

        [HttpGet]
        [Route("Product/{productId}/Type/{isIssued}")]
        public IActionResult GetStorages(int productId, bool isIssued)
        {
            var results = _storageService.GetStorages(productId, isIssued);
            return StatusCode(200, results);
        }

        [HttpGet]
        [Route("Type/{isIssued}")]
        public IActionResult GetStorages(bool isIssued)
        {
            var results = _storageService.GetStorages(isIssued);
            return StatusCode(200, results);
        }

        [HttpGet]
        [Route("{productId}/Order/{orderId}/Type/{isIssued}")]
        public IActionResult GetStorages(int productId, int orderId, bool isIssued)
        {
            var results = _storageService.GetStorages(productId, isIssued, orderId);
            return StatusCode(200, results);
        }

        [HttpGet]
        [Route("Product/{productId}/Time/{day}/{month}/{year}")]
        public IActionResult GetStorages(int? productId, int day, int month, int? year)
        {
            var results = _storageService.GetStorages(productId, day, month, year);
            return StatusCode(200, results);
        }

        [HttpGet]
        public IActionResult GetStorage(int id)
        {
            var result = _storageService.GetStorage(id);
            return StatusCode(200, result);
        }

        [HttpPost]
        public IActionResult GetDailyReport([FromBody] DateTime date)
        {
            var result = _storageService.GetDailyReport(date);
            return StatusCode(200, result);
        }
    }
}
