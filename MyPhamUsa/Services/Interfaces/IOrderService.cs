using System;
using System.Collections;
using System.Collections.Generic;
using MyPhamUsa.Models.ViewModels;
namespace MyPhamUsa.Services.Interfaces
{
    public interface IOrderService
    {
        #region For Client
        OrderViewModel GetOrder(int id);
        ICollection<OrderViewModel> GetOrders();
        int CreateOrder(OrderCreateViewModel newOrder);
        bool DeleteOrder(int orderId);
        #endregion

    }
}
