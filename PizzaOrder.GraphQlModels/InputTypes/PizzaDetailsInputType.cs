using GraphQL.Types;
using PizzaOrder.Business.Models;
using PizzaOrder.GraphQlModels.Enums;

namespace PizzaOrder.GraphQlModels.InputTypes
{
    public sealed class PizzaDetailsInputType:InputObjectGraphType<PizzaDetailsModel>
    {
        public PizzaDetailsInputType()
        {
            Field(x => x.Name);
            Field(x => x.Size);
            Field(x => x.Price);
            Field<ToppingsEnumType>("toppings");
        }
    }
}