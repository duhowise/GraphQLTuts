using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PizzaOrder.Data;

namespace PizzaOrder.Business.Services
{
    public class PizzaDetailService: IPizzaDetailService
    {
        private readonly PizzaDbContext _context;

        public PizzaDetailService(PizzaDbContext context)
        {
            _context = context;
        }

        public async Task<PizzaDetail> GetPizzaDetail(int pizzaDetailId)
        {
            return await _context.PizzaDetails.FindAsync(pizzaDetailId);
        }

        public async Task<IEnumerable<PizzaDetail>> GetAllPizzaDetailsForOrder(int orderDetailId)
        {
            return await _context.PizzaDetails.Where(x => x.OrderDetailId == orderDetailId).ToListAsync();
        }

        public async Task<IEnumerable<PizzaDetail>> CreateBulkAsync(IEnumerable<PizzaDetail> pizzaDetails,int orderId)
        {
            _context.PizzaDetails.AddRange(pizzaDetails);
           await _context.SaveChangesAsync();
           return await _context.PizzaDetails.Where(x=>x.OrderDetailId==orderId).ToListAsync();
        }

        public async Task<int> DeletePizzaDetailsAsync(int pizzaDetailsId)
        {
            var pizzaDetail = await _context.PizzaDetails.FindAsync(pizzaDetailsId);
            if (pizzaDetail == null) return 0;
            var orderId = pizzaDetail.OrderDetailId;
            _context.PizzaDetails.Remove(pizzaDetail);
            await _context.SaveChangesAsync();
            return orderId;

        }
    }
}