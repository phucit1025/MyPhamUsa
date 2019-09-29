using MyPhamUsa.Models.ViewModels;
using System;

namespace MyPhamUsa.Services.Interfaces
{
    public interface IStatisticsService
    {
        string GetDayTotalMoney(DateTime date, bool isIssue);
        string GetMonthTotalMoney(DateTime date, bool isIssue);
        string GetWeekTotalMoney(DateTime date, bool isIssue);
        string GetTotalMoneyFromTo(DateTime from, DateTime to, bool isIssue);
        CurrentStorageReport GetCurrentStorageValue();
    }
}
