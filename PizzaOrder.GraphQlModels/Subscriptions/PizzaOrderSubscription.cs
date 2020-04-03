using System.Reactive.Linq;
using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Types;
using PizzaOrder.Business.Models;
using PizzaOrder.Business.Services;
using PizzaOrder.Data;
using PizzaOrder.GraphQlModels.Enums;
using PizzaOrder.GraphQlModels.Types;

namespace PizzaOrder.GraphQlModels.Subscriptions
{
    public sealed class PizzaOrderSubscription: ObjectGraphType
    {
        public PizzaOrderSubscription(IEventService eventService)
        {
            Name = nameof(PizzaOrderSubscription);
            AddField(new EventStreamFieldType
            {
                Name = "orderCreated",
                Description = "This service listens for updates from newly created orders",
                Type = typeof(EventDataType),
                Resolver = new FuncFieldResolver<EventDataModel>(context=>context.Source as EventDataModel),
                Subscriber = new EventStreamResolver<EventDataModel>(context => eventService.OnCreateObservable)
            });

            AddField(new EventStreamFieldType
            {
                Name = 
                    "statusUpdate",
                Arguments = new QueryArguments(new QueryArgument<NonNullGraphType<OrderStatusEnumType>>{Name = "status"}),
                Type = typeof(EventDataType),
                Resolver = new FuncFieldResolver<EventDataModel>(ResolveMessage),
                Subscriber = new EventStreamResolver<EventDataModel>(context =>
                {
                    var status = context.GetArgument<OrderStatus>("status");
                    var events = eventService.OnStatusUpdateObservable();
                    return events.Where(x => x.OrderStatus == status);
                })
            });
        }

        private static EventDataModel ResolveMessage(IResolveFieldContext context)
        {
            var eventData = context.Source as EventDataModel;
            return eventData;
        }
    }
}