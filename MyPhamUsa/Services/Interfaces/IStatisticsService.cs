using System;
using System.Collections;
using MyPhamUsa.Models.ViewModels;

namespace MyPhamUsa.Services.Interfaces
{
    public interface IStatisticsService
    {
        string GetDayTotalMoney(DateTime date, bool isIssue);
    }
}
