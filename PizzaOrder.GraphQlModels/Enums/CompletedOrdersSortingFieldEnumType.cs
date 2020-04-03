using GraphQL.Types;
using PizzaOrder.Business.Models;

namespace PizzaOrder.GraphQlModels.Enums
{
    public class  CompletedOrdersSortingFieldEnumType : EnumerationGraphType<CompletedOrderSortingFields>
    {
        public CompletedOrdersSortingFieldEnumType()
        {
            Name = nameof(CompletedOrdersSortingFieldEnumType);
        }
    }
}