namespace PizzaOrder.Business.Models
{
    public class SortingDetails<T>
    {
        public T Field { get; set; }
        public SortingDirection SortingDirection { get; set; }
    }
}