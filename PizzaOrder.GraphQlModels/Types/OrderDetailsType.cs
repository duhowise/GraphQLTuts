using GraphQL.Types;
using PizzaOrder.Business.Services;
using PizzaOrder.Data;
using PizzaOrder.GraphQlModels.Enums;

namespace PizzaOrder.GraphQlModels.Types
{
    public sealed class OrderDetailsType:ObjectGraphType<OrderDetail>
    {
        public OrderDetailsType(IPizzaDetailService pizzaDetailService)
        {
            Name = nameof(OrderDetailsType);
            Field(x => x.Id);
            Field(x => x.AddressLine1);
            Field(x => x.AddressLine2);
            Field(x => x.MobileNumber);
            Field(x => x.Amount);
            Field(x => x.Date);
            Field<OrderStatusEnumType>(name: "orderStatus", resolve: context => context.Source.OrderStatus);
            Field<ListGraphType<PizzaDetailsType>>(name: "pizzaDetails", resolve: context =>pizzaDetailService.GetAllPizzaDetailsForOrder(context.Source.Id));
        }
    }
}