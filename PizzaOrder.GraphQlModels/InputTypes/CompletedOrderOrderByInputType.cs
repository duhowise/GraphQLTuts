using GraphQL.Types;
using GraphQL.Utilities.Federation;
using PizzaOrder.Business.Models;
using PizzaOrder.GraphQlModels.Enums;

namespace PizzaOrder.GraphQlModels.InputTypes
{
    public class CompletedOrderOrderByInputType:InputObjectGraphType<SortingDetails<CompletedOrderSortingFields>>
    {
        public CompletedOrderOrderByInputType()
        {
            Field<CompletedOrdersSortingFieldEnumType>("field", resolve: context => context.Source.Field);
            Field<SortingDirectionEnumType>("direction", resolve: context => context.Source.SortingDirection);
        }
    }
}