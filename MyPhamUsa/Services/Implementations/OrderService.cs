using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyPhamUsa.Data;
using MyPhamUsa.Models.Entities;
using MyPhamUsa.Models.ViewModels;
using MyPhamUsa.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyPhamUsa.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public OrderService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int CreateOrder(OrderCreateViewModel newOrder)
        {
            var tracker = _context.Database.BeginTransaction();

            try
            {

                #region Create Order
                var order = _mapper.Map<OrderCreateViewModel, Order>(newOrder);
                _context.Orders.Add(order);
                _context.SaveChanges();
                #endregion

                #region Add Order Items
                var items = new List<Storage>();

                foreach (var orderItem in newOrder.OrderItems)
                {
                    #region Issue to Order
                    items.Add(new Storage()
                    {
                        ProductId = orderItem.ProductId,
                        Quantity = orderItem.Quantity,
                        OrderId = order.Id,
                        IsIssued = true,
                        Description = "Xuất Hàng cho Order online"
                    });
                    #endregion

                    #region Change Quantity Index
                    var product = _context.Products.Find(orderItem.ProductId);
                    product.QuantityIndex -= orderItem.Quantity;
                    product.DateUpdated = DateTime.Now;
                    _context.Update(product);
                    #endregion
                }
                _context.Storages.AddRange(items);
                _context.SaveChanges();
                #endregion

                tracker.Commit();
                return order.Id;
            }
            catch (DbUpdateException)
            {
                tracker.Rollback();
                return 0;
            }


        }

        public bool DeleteOrder(int orderId)
        {
            var tracker = _context.Database.BeginTransaction();
            try
            {
                var order = _context.Orders.Find(orderId);
                foreach (var item in order.OrderItems)
                {
                    item.DateUpdated = DateTime.Now;
                    item.IsIssued = true;
                    item.Description = "Nhập lại kho do huỷ Order";
                    item.OrderId = null;
                    _context.Update(item);

                    item.Product.QuantityIndex += item.Quantity;
                    _context.Update(item.Product);
                }
                _context.SaveChanges();

                _context.Orders.Remove(order);
                _context.SaveChanges();

                tracker.Commit();
                return true;
            }
            catch (DbUpdateException)
            {
                tracker.Rollback();
                return false;
            }

        }

        public OrderViewModel GetOrder(int id)
        {
            var order = _context.Orders.Find(id);
            return _mapper.Map<Order, OrderViewModel>(order);
        }

        public ICollection<OrderViewModel> GetOrders(DateTime? time)
        {
            var orders = new List<Order>();
            if (time.HasValue)
            {
                orders = _context.Orders.Where(o => o.DateCreated.ToShortDateString().Equals(time.Value.ToShortDateString())).ToList();
            }
            else
            {
                orders = _context.Orders.ToList();

            }

            if (orders.Any())
            {
                var results = _mapper.Map<List<Order>, List<OrderViewModel>>(orders);
                return results;
            }

            return new List<OrderViewModel>();

        }
    }
}
