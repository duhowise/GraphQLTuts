namespace PizzaOrder.Business.Helpers
{
    public static class Constants
    {
        public static class Roles
        {
            public static string Customer { get; set; } = "Customer";
            public static string Restaurant { get; set; } = "Restaurant";
            public static string Admin { get; set; } = "Admin";
        }
        public static class AuthPolicy
        {
            public static string CustomerPolicy { get; set; } = "CustomerPolicy";
            public static string RestaurantPolicy { get; set; } = "RestaurantPolicy";
            public static string AdminPolicy { get; set; } = "AdminPolicy";
        }

        
    }
}