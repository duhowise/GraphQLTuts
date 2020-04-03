using GraphQL.Types;

namespace PizzaOrder.GraphQlModels.Enums
{
    public class OrderStatusEnumType:EnumerationGraphType
    {
        public OrderStatusEnumType()
        {
            Name = "orderStatus";
            AddValue("Created","Order was Created",1);
            AddValue("InKitchen","Order is being Prepared",2);
            AddValue("OnTheWay","Order is on the way",3);
            AddValue("Delivered","Order was Delivered",4);
            AddValue("Cancelled","Order is Cancelled",5);
        }
    }
}