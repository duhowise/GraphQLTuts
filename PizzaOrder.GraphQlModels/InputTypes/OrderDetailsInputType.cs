using GraphQL.Types;
using PizzaOrder.Business.Models;

namespace PizzaOrder.GraphQlModels.InputTypes
{
    public sealed class OrderDetailsInputType:InputObjectGraphType<OrderDetailsModel>
    {
        public OrderDetailsInputType()
        {
            Field(x => x.AddressLine1);
            Field(x => x.AddressLine2);
            Field(x => x.Amount);
            Field(x => x.MobileNumber);
            Field<ListGraphType<PizzaDetailsInputType>>("pizzaDetails");
        }
    }
}