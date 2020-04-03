using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;

namespace PizzaOrder.Data
{
    public static class DbContextExtensions
    {
        public static void EnsureDataSeeded(this PizzaDbContext dbContext)
        {
            if (!dbContext.OrderDetails.Any())
            {
                dbContext.OrderDetails.AddRange(new List<OrderDetail>
                {
                    new OrderDetail("4881 Thrash Trail","Long View","12345678909",100),
                    new OrderDetail("4973 CrestView Terrace","San Antonio","12345678909",180),
                    new OrderDetail("4019 Burwell Heights","Sugar land","12345678909",50),
                    new OrderDetail("2280 Charmaine lane","lubbock","12345678909",120),
                });
                dbContext.SaveChanges();
            } 
            
            if (!dbContext.PizzaDetails.Any())
            {
                dbContext.PizzaDetails.AddRange(new List<PizzaDetail>
                {
                    new PizzaDetail("NeaPolitan Pizza",Toppings.ExtraCheese|Toppings.Onions,100,11,1),
                    new PizzaDetail("Greek Pizza",Toppings.Mushrooms|Toppings.Pepperoni,120,10,3),
                    new PizzaDetail("New York Style Pizza",Toppings.Sausage,80,11,2),
                   
                });
                dbContext.SaveChanges();
            }
        }
    }
}