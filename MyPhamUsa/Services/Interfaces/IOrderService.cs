using MyPhamUsa.Models.ViewModels;
using System.Collections.Generic;
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
