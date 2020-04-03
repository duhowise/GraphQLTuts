using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using GraphQL.Types;
using PizzaOrder.Business.Services;
using PizzaOrder.GraphQlModels.Types;
using GraphQL;
using GraphQL.Authorization;
using GraphQL.Types.Relay.DataObjects;
using PizzaOrder.Business.Helpers;
using PizzaOrder.Business.Models;
using PizzaOrder.Data;
using PizzaOrder.GraphQlModels.InputTypes;

namespace PizzaOrder.GraphQlModels.Queries
{
    public class PizzaOrderQuery:ObjectGraphType
    {

        public PizzaOrderQuery(IOrderDetailService orderDetailService, IPizzaDetailService pizzaDetailService)
        {
            Name = nameof(PizzaOrderQuery);
            FieldAsync<ListGraphType<OrderDetailsType>>(name: "newOrders",resolve:
                async context => await orderDetailService.GetAllNewOrdersAsync()).AuthorizeWith(Constants.AuthPolicy.RestaurantPolicy);

            FieldAsync<PizzaDetailsType>("pizzaDetails", arguments: new QueryArguments(new QueryArgument<IntGraphType>
            {
                Name = "id"
            }), resolve: async context => await pizzaDetailService.GetPizzaDetail(context.GetArgument<int>("id")));
     
            FieldAsync<OrderDetailsType>("orderDetails", arguments: new QueryArguments(new QueryArgument<IntGraphType>
            {
                Name = "id"
            }), resolve: async context => await orderDetailService.GetOrderDetailAsync(context.GetArgument<int>("id")));

            Connection<OrderDetailsType>().Name("completedOrder")
                .Unidirectional()
                .PageSize(10)
                .Argument<CompletedOrderOrderByInputType>("orderBy","pass in field and direction in which you want to sort data")
                .ResolveAsync(async context =>
                {
                    var pagedRequested=new PagedRequest
                    {
                        First = context.First,
                        Last = context.Last,
                        Before = context.Before,
                        After = context.After,
                        OrderBy = context.GetArgument<SortingDetails<CompletedOrderSortingFields>>("orderBy")
                        
                    };

                    var pagedResponse = await orderDetailService.GetCompletedOrdersAsync(pagedRequested);
                    var (startingCursor, endingCursor) =
                        CursorHelper.GetFirstAndLastCursor(pagedResponse.Nodes.Select(x => x.Id).ToList());
                    var edge = pagedResponse.Nodes.Select(x => new Edge<OrderDetail>
                    {
                        Cursor = CursorHelper.ToCursor(x.Id),
                        Node = x
                    }).ToList();
                    var connection=new Connection<OrderDetail>
                    {
                        Edges = edge,
                        TotalCount = pagedResponse.TotalCount,
                        PageInfo = new PageInfo
                        {
                            HasNextPage = pagedResponse.HasNextPage,
                             HasPreviousPage = pagedResponse.HasPreviousPage,
                             EndCursor = endingCursor,
                             StartCursor = startingCursor
                        }

                    };
                    return connection;
                });
        }

       
    }
}