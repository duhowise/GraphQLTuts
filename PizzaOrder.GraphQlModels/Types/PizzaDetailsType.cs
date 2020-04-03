using GraphQL.Types;
using PizzaOrder.Data;
using PizzaOrder.GraphQlModels.Enums;

namespace PizzaOrder.GraphQlModels.Types
{
    public sealed class PizzaDetailsType:ObjectGraphType<PizzaDetail>
    {
        public PizzaDetailsType()
        {
            Name = nameof(PizzaDetailsType);
            Field(x => x.Id);
            Field(x => x.Name);
            Field(x => x.Price);
            Field(x => x.Size);
            Field(x => x.OrderDetailId);
            Field<ToppingsEnumType>(name: "toppings", resolve: context => context.Source.Toppings);
        }
    }
}