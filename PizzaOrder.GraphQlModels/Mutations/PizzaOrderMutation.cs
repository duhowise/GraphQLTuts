using System.Linq;
using GraphQL;
using GraphQL.Types;
using PizzaOrder.Business.Models;
using PizzaOrder.Business.Services;
using PizzaOrder.Data;
using PizzaOrder.GraphQlModels.Enums;
using PizzaOrder.GraphQlModels.InputTypes;
using PizzaOrder.GraphQlModels.Types;

namespace PizzaOrder.GraphQlModels.Mutations
{
    public class PizzaOrderMutation:ObjectGraphType
    {
        public PizzaOrderMutation(IPizzaDetailService pizzaDetailService,IOrderDetailService orderDetailService)
        {
            Name = nameof(PizzaOrderMutation);
            FieldAsync<OrderDetailsType>(
                
                name:"createOrder",
                arguments:new QueryArguments(new QueryArgument<NonNullGraphType<OrderDetailsInputType>>
                {
                    Name = "orderDetails"
                })
                ,resolve:
                async context =>
                {
                    var order = context.GetArgument<OrderDetailsModel>("orderDetails");
                    var orderDetails=new OrderDetail(order.AddressLine1,order.AddressLine2,order.MobileNumber,order.Amount);
                    orderDetails = await orderDetailService.CreateAsync(orderDetails);
                    var pizzaDetails = order.PizzaDetails.Select(x=>new PizzaDetail(x.Name,x.Toppings,x.Price,x.Size,orderDetails.Id));
                    pizzaDetails = await pizzaDetailService.CreateBulkAsync(pizzaDetails, orderDetails.Id);
                    orderDetails.PizzaDetails = pizzaDetails.ToList();
                    return orderDetails;
                });
            FieldAsync<OrderDetailsType>(
                name:"updateStatus",
                arguments:new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> {
                        Name = "id"},
                    new QueryArgument<NonNullGraphType<OrderStatusEnumType>> { Name = "status"}
                    ),
                resolve: async context =>
                {
                    int orderId = context.GetArgument<int>("id");
                    OrderStatus orderStatus = context.GetArgument<OrderStatus>("status");
                  return await orderDetailService.UpdateStatusAsync(orderId, orderStatus);
                }
                );


            FieldAsync<OrderDetailsType>(
                name:"deletePizzaDetails",
                arguments:new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name ="pizzaDetailsId"}
                    ),
                resolve: async context =>
                {
                    var pizzaDetailsId = context.GetArgument<int>("pizzaDetailsId");
                   var orderId= await pizzaDetailService.DeletePizzaDetailsAsync(pizzaDetailsId);
                   return await orderDetailService.GetOrderDetailAsync(orderId);
                }
                );
        }
    }
}