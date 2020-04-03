using System.Collections.Generic;
using System.Threading.Tasks;
using PizzaOrder.Business.Models;
using PizzaOrder.Data;

namespace PizzaOrder.Business.Services
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetail>> GetAllNewOrdersAsync();
        Task<OrderDetail> GetOrderDetailAsync(int orderId);
        Task<OrderDetail> CreateAsync(OrderDetail orderDetail);
        Task<OrderDetail> UpdateStatusAsync(int orderId, OrderStatus status);
        Task<PagedResponse<OrderDetail>> GetCompletedOrdersAsync(PagedRequest request);
    }
}