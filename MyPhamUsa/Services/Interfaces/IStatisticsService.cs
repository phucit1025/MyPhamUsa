using MyPhamUsa.Models.ViewModels;
using System;

namespace MyPhamUsa.Services.Interfaces
{
    public interface IStatisticsService
    {
        StatisticsViewModel GetDayTotalMoney(DateTime date, bool isIssue);
        StatisticsViewModel GetMonthTotalMoney(DateTime date, bool isIssue);
        StatisticsViewModel GetWeekTotalMoney(DateTime date, bool isIssue);
        StatisticsViewModel GetTotalMoneyFromTo(DateTime from, DateTime to, bool isIssue);
        CurrentStorageReport GetCurrentStorageValue();
    }
}
