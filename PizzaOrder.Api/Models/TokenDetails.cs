using System;

namespace PizzaOrder.Api.Models
{
    public class TokenDetails
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpireOn { get; set; }
    }
}