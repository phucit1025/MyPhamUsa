using Microsoft.AspNetCore.Mvc;
using MyPhamUsa.Services.Interfaces;
using System;

namespace MyPhamUsa.Controllers
{
    [Route("api/Statistics/[action]")]
    public class StatisticsController : Controller
    {
        private readonly IStatisticsService _statisticsService;
        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet]
        public IActionResult GetCurrentStorage()
        {
            var report = _statisticsService.GetCurrentStorageValue();
            return StatusCode(200, report);
        }

        [HttpGet]
        public IActionResult GetDayMoney(DateTime date, bool isIssue)
        {
            var report = _statisticsService.GetDayTotalMoney(date, isIssue);
            return StatusCode(200, report);
        }

        [HttpGet]
        public IActionResult GetMonthMoney(DateTime date, bool isIssue)
        {
            var report = _statisticsService.GetMonthTotalMoney(date, isIssue);
            return StatusCode(200, report);
        }

        [HttpGet]
        public IActionResult GetWeekMoney(DateTime date, bool isIssue)
        {
            var report = _statisticsService.GetWeekTotalMoney(date, isIssue);
            return StatusCode(200, report);
        }

        [HttpGet]
        public IActionResult GetMoneyFromTo(DateTime start, DateTime end, bool isIssue)
        {
            var report = _statisticsService.GetTotalMoneyFromTo(start, end, isIssue);
            return StatusCode(200, report);
        }
    }
}
