using AutoMapper;
using MyPhamUsa.Client.Data;
using MyPhamUsa.Client.Models.Entities;
using MyPhamUsa.Client.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPhamUsa.Client.Services
{
    public interface IOrderService
    {
        ResultViewModel CreateOrder(OrderCreateViewModel model);
    }

    public class OrderService : IOrderService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ResultViewModel CreateOrder(OrderCreateViewModel model)
        {
            var result = new ResultViewModel();
            var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                #region Create Order
                var newOrder = new Order
                {
                    CustomerName = model.CustomerName,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber
                };
                _dbContext.Add(newOrder);
                _dbContext.SaveChanges();

                var newOrderItems = _mapper.Map<List<OrderItemCreateViewModel>, List<OrderItem>>(model.Items, opt =>
                {
                    opt.AfterMap((src, dst) =>
                    {
                        foreach (var dstItem in dst)
                        {
                            dstItem.OrderId = newOrder.Id;
                            dstItem.OrderPrice = _dbContext.Products.Find(dstItem.Id).SellPrice;
                        }
                    });
                });
                _dbContext.OrderItems.AddRange(newOrderItems);
                _dbContext.SaveChanges();
                #endregion

                #region Issue from Storage
                var parallelLoopResult = Parallel.ForEach(newOrderItems, (newOrderItem, state) =>
                {
                    var product = _dbContext.Products.Find(newOrderItem.ProductId);
                    if (product.QuantityIndex < newOrderItem.Quantity)
                    {
                        state.Break();
                    }
                    else
                    {
                        var issueModel = new Storage
                        {
                            Quantity = newOrderItem.Quantity,
                            ProductId = newOrderItem.ProductId,
                            IsIssued = true,
                            Description = $"*Đợi Xác Nhận* Bán cho đơn hàng có Mã : {newOrder}"
                        };
                        _dbContext.Add(issueModel);
                    }
                });
                if (parallelLoopResult.IsCompleted)
                {
                    _dbContext.SaveChanges();
                }
                else // There is a product which no longer in stock. ( eg. maybe the product is being issued to another order and waiting for approval )
                {
                    transaction.Rollback();
                    result.ErrorMessage = "Món hàng hiện tại đã hết.";
                    return result;
                }
                #endregion

                result.Succeed = true;
                return result;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                return result;
            }
            finally
            {
                transaction.Dispose();
            }
        }
    }
}
