using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MyPhamUsa.Data;
using MyPhamUsa.Models.Entities;
using MyPhamUsa.Services.Interfaces;

namespace MyPhamUsa.Services.Implementations
{
    public class StatisticsService:IStatisticsService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public StatisticsService(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string GetDayTotalMoney(DateTime date,bool isIssue)
        {
            var transactions = new List<Storage>();
            if (isIssue)
            {
                transactions = _context.Storages.Where(s => !s.IsDeleted && s.IsIssued && s.DateUpdated.Day == date.Day && s.DateUpdated.Month == date.Month).ToList();
            }
            else
            {
                transactions = _context.Storages.Where(s => !s.IsDeleted && !s.IsIssued && s.DateUpdated.Day == date.Day && s.DateUpdated.Month == date.Month).ToList();
            }
            if (transactions.Any())
            {
                var total = 0;
                foreach (var transaction in transactions)
                {
                    total += (transaction.Quantity * Convert.ToInt32(transaction.Product.Price));
                }
                return total.ToString();
            }

            return "0";
        }
    }
}
