using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PizzaOrder.Data
{
    public class PizzaDbContext:IdentityDbContext
    {
        public PizzaDbContext(DbContextOptions<PizzaDbContext> options):base(options)
        {
            
        }

        public DbSet<OrderDetail> OrderDetails{ get; set; }
        public DbSet<PizzaDetail> PizzaDetails { get; set; }
        
    }
}
