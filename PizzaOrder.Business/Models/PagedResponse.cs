using System.Collections.Generic;

namespace PizzaOrder.Business.Models
{
    public class PagedResponse<T>
    {
        public List<T> Nodes{ get; set; }
        public int TotalCount { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }

    }
}