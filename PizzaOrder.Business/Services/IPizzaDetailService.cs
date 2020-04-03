using System.Collections.Generic;
using System.Threading.Tasks;
using PizzaOrder.Data;

namespace PizzaOrder.Business.Services
{
    public interface IPizzaDetailService
    {
        Task<PizzaDetail> GetPizzaDetail(int pizzaDetailId);
        Task<IEnumerable<PizzaDetail>> GetAllPizzaDetailsForOrder(int orderDetailId);
        Task<IEnumerable<PizzaDetail>> CreateBulkAsync(IEnumerable<PizzaDetail> pizzaDetails, int orderId);
        Task<int> DeletePizzaDetailsAsync(int pizzaDetailsId);
    }
}