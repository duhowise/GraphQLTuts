using GraphQL.Types;
using PizzaOrder.Data;

namespace PizzaOrder.GraphQlModels.Enums
{
    public class ToppingsEnumType:EnumerationGraphType<Toppings>
    {
        public ToppingsEnumType()
        {
            Name = nameof(ToppingsEnumType);


        }
    }
}