using AutoMapper;
using MyPhamUsa.Data;
using MyPhamUsa.Extensions;
using MyPhamUsa.Models.Entities;
using MyPhamUsa.Models.ViewModels;
using MyPhamUsa.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyPhamUsa.Services.Implementations
{
    public class StatisticsService : IStatisticsService
    {
        private readonly AppDbContext _context;

        public StatisticsService(AppDbContext context)
        {
            _context = context;
        }

        public CurrentStorageReport GetCurrentStorageValue()
        {
            var result = new CurrentStorageReport();
            var products = _context.Products.Where(p => !p.IsDeleted && p.QuantityIndex > 0);

            long totalSellPrice = 0;
            long totalPrice = 0;
            int totalQuantity = 0;
            foreach (var product in products)
            {
                totalSellPrice += product.QuantityIndex * long.Parse(product.SellPrice);
                totalPrice += product.QuantityIndex * long.Parse(product.Price);
                totalQuantity += product.QuantityIndex;
            }

            result.TotalSellPrice = totalSellPrice.ToString();
            result.TotalPrice = totalPrice.ToString();
            result.TotalQuantity = totalQuantity.ToString();
            return result;
        }

        public StatisticsViewModel GetDayTotalMoney(DateTime date, bool isIssue)
        {
            var transactions = _context.Storages.Where(s => !s.IsDeleted && !s.Product.IsDeleted && s.IsIssued == isIssue && s.DateUpdated.Day == date.Day && s.DateUpdated.Month == date.Month).ToList();

            return GetTotalMoney(transactions);
        }

        public StatisticsViewModel GetMonthTotalMoney(DateTime date, bool isIssue)
        {
            var transactions = _context.Storages.Where(s => !s.IsDeleted
                                                           && !s.Product.IsDeleted
                                                           && s.IsIssued == isIssue
                                                           && s.DateCreated.Year == date.Year
                                                           && s.DateCreated.Month == date.Month).ToList();
            return GetTotalMoney(transactions);
        }

        public StatisticsViewModel GetTotalMoneyFromTo(DateTime from, DateTime to, bool isIssue)
        {
            var transactions = _context.Storages.Where(s => !s.IsDeleted
                                                            && !s.Product.IsDeleted
                                                            && s.IsIssued == isIssue
                                                            && s.DateCreated >= from.Midnight()
                                                            && s.DateCreated <= to.Latest()).ToList();
            return GetTotalMoney(transactions);
        }

        public StatisticsViewModel GetWeekTotalMoney(DateTime date, bool isIssue)
        {
            var startOfWeekDate = date.GetDateOfDayOfWeek(DayOfWeek.Monday);
            var endOfWeekDate = date.GetDateOfDayOfWeek(DayOfWeek.Sunday);

            var transactions = _context.Storages.Where(s => !s.IsDeleted
                                                           && !s.Product.IsDeleted
                                                           && s.IsIssued == isIssue
                                                           && s.DateCreated >= startOfWeekDate
                                                           && s.DateCreated <= endOfWeekDate
                                                           ).ToList();
            return GetTotalMoney(transactions);
        }

        private StatisticsViewModel GetTotalMoney(List<Storage> transactions)
        {
            var result = new StatisticsViewModel();
            if (transactions.Any())
            {
                var totalPrice = 0;
                var totalSellPrice = 0;
                foreach (var transaction in transactions)
                {
                    totalPrice += transaction.Quantity * Convert.ToInt32(transaction.Product.Price);
                    totalSellPrice += transaction.Quantity * Convert.ToInt32(transaction.Product.SellPrice);
                }
                result.Price = totalPrice.ToString();
                result.SellPrice = totalSellPrice.ToString();
            }
            return result;
        }
    }
}
