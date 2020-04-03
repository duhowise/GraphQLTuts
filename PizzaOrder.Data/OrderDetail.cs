using System;
using System.Collections.Generic;

namespace PizzaOrder.Data
{
    public class OrderDetail
    {
        protected OrderDetail()
        {
            
        }
        public OrderDetail( string addressLine1, string addressLine2, string mobileNumber, decimal amount)
        {
           AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            MobileNumber = mobileNumber;
            Amount = amount;
            Date = DateTime.Now;
            OrderStatus = OrderStatus.Created;
        }
        public int Id { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string MobileNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public ICollection<PizzaDetail> PizzaDetails { get; set; }
    }
}