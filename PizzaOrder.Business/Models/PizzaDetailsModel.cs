using PizzaOrder.Data;

namespace PizzaOrder.Business.Models
{
    public class PizzaDetailsModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Size { get; set; }
        public Toppings Toppings { get; set; }

    }
}