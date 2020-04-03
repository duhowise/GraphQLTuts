using System.Collections.Generic;

namespace PizzaOrder.Business.Models
{
    public class OrderDetailsModel
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string MobileNumber { get; set; }
        public decimal Amount { get; set; }
        public List<PizzaDetailsModel> PizzaDetails { get; set; }
    }
}