namespace PizzaOrder.Business.Models
{
    public class PagedRequest
    {
        public int? First { get; set; }
        public int? Last { get; set; }
        public string Before { get; set; }
        public string After { get; set; }
        public SortingDetails<CompletedOrderSortingFields> OrderBy { get; set; }
    }
}