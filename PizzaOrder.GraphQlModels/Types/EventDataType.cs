using GraphQL.Types;
using PizzaOrder.Business.Models;
using PizzaOrder.GraphQlModels.Enums;

namespace PizzaOrder.GraphQlModels.Types
{
    public sealed class EventDataType:ObjectGraphType<EventDataModel>
    {
        public EventDataType()
        {
            Name = nameof(EventDataType);
            Field(x => x.OrderId);
            Field<OrderStatusEnumType>("orderStatus",resolve:context=>context.Source.OrderStatus);
        }
    }
}