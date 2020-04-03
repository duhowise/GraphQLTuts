namespace PizzaOrder.Data
{
    public class PizzaDetail
    {
        public PizzaDetail( string name, Toppings toppings, decimal price, decimal size, int orderDetailId)
        {
            Name = name;
            Toppings = toppings;
            Price = price;
            Size = size;
            OrderDetailId = orderDetailId;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public Toppings Toppings { get; set; }
        public decimal Price { get; set; }
        public decimal Size { get; set; }
        public int OrderDetailId { get; set; }
        public OrderDetail OrderDetail { get; set; }
    }
}