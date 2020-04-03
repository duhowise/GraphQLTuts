using GraphQL.Types;
using PizzaOrder.Business.Models;

namespace PizzaOrder.GraphQlModels.InputTypes
{
    public class SortingDirectionEnumType:EnumerationGraphType<SortingDirection>
    {
        public SortingDirectionEnumType()
        {
            Name = nameof(SortingDirectionEnumType);
        }
    }
}