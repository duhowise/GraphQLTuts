using System;
using PizzaOrder.GraphQlModels.Mutations;
using PizzaOrder.GraphQlModels.Queries;
using PizzaOrder.GraphQlModels.Subscriptions;

namespace PizzaOrder.GraphQlModels.Schema
{
    public class PizzaOrderSchema:GraphQL.Types.Schema
    {
        public PizzaOrderSchema(IServiceProvider serviceProvider):base(serviceProvider)
        {
            Services = serviceProvider;
            Query = (PizzaOrderQuery)serviceProvider.GetService(typeof(PizzaOrderQuery));
            Mutation = (PizzaOrderMutation)serviceProvider.GetService(typeof(PizzaOrderMutation));
            Subscription = (PizzaOrderSubscription) serviceProvider.GetService(typeof(PizzaOrderSubscription));
        }
    }
}