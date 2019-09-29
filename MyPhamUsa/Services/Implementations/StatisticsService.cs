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
        private readonly IMapper _mapper;

        public StatisticsService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public CurrentStorageReport GetCurrentStorageValue()
        {
            var result = new CurrentStorageReport();
            var products = _context.Products.Where(p => !p.IsDeleted && p.QuantityIndex > 0);

            long totalPrice = 0;
            int totalQuantity = 0;
            foreach (var product in products)
            {
                totalPrice += (product.QuantityIndex * long.Parse(product.SellPrice));
                totalQuantity += product.QuantityIndex;
            }

            result.TotalPrice = totalPrice.ToString();
            result.TotalQuantity = totalQuantity.ToString();
            return result;
        }

        public string GetDayTotalMoney(DateTime date, bool isIssue)
        {
            var transactions = _context.Storages.Where(s => !s.IsDeleted && s.IsIssued == isIssue && s.DateUpdated.Day == date.Day && s.DateUpdated.Month == date.Month).ToList();

            return GetTotalMoney(transactions);
        }

        public string GetMonthTotalMoney(DateTime date, bool isIssue)
        {
            var transactions = _context.Storages.Where(s => !s.IsDeleted
                                                           && s.IsIssued == isIssue
                                                           && s.DateCreated.Year == date.Year
                                                           && s.DateCreated.Month == date.Month).ToList();
            return GetTotalMoney(transactions);
        }

        public string GetTotalMoneyFromTo(DateTime from, DateTime to, bool isIssue)
        {
            var transactions = _context.Storages.Where(s => !s.IsDeleted
                                                            && s.IsIssued == isIssue
                                                            && s.DateCreated >= from.Midnight()
                                                            && s.DateCreated <= to.Latest()).ToList();
            return GetTotalMoney(transactions);
        }

        public string GetWeekTotalMoney(DateTime date, bool isIssue)
        {
            var startOfWeekDate = date.GetDateOfDayOfWeek(DayOfWeek.Monday);
            var endOfWeekDate = date.GetDateOfDayOfWeek(DayOfWeek.Sunday);

            var transactions = _context.Storages.Where(s => !s.IsDeleted
                                                           && s.IsIssued == isIssue
                                                           && s.DateCreated >= startOfWeekDate
                                                           && s.DateCreated <= endOfWeekDate
                                                           ).ToList();
            return GetTotalMoney(transactions);
        }

        private string GetTotalMoney(List<Storage> transactions)
        {
            if (transactions.Any())
            {
                var total = 0;
                foreach (var transaction in transactions)
                {
                    total += (transaction.Quantity * Convert.ToInt32(transaction.Product.SellPrice));
                }
                return total.ToString();
            }
            return "0";
        }
    }
}
