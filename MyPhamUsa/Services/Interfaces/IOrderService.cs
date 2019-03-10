using MyPhamUsa.Models.ViewModels;
using System;
using System.Collections.Generic;
namespace MyPhamUsa.Services.Interfaces
{
    public interface IOrderService
    {
        #region For Client
        OrderViewModel GetOrder(int id);
        int CreateOrder(OrderCreateViewModel newOrder);
        bool DeleteOrder(int orderId);
        #endregion

        ICollection<OrderViewModel> GetOrders(DateTime? time);

    }
}
