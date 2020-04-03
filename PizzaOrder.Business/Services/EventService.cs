using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using PizzaOrder.Business.Models;

namespace PizzaOrder.Business.Services
{
    public interface IEventService
    {
        void CreateOrderEvent(EventDataModel orderEvent);
        void StatusUpdateEvent(EventDataModel orderEvent);
        IObservable<EventDataModel> OnCreateObservable { get; }
        IObservable<EventDataModel> OnStatusUpdateObservable();
    }

    public class EventService : IEventService
    {
        private readonly ISubject<EventDataModel> _onCreateSubject;
        private readonly ISubject<EventDataModel> _onStatusUpdateSubject;

        public EventService()
        {
            _onCreateSubject = new ReplaySubject<EventDataModel>(1);
            _onStatusUpdateSubject = new ReplaySubject<EventDataModel>(1);
        }

        public void CreateOrderEvent(EventDataModel orderEvent) => _onCreateSubject.OnNext(orderEvent);
        public void StatusUpdateEvent(EventDataModel orderEvent) => _onStatusUpdateSubject.OnNext(orderEvent);

        public IObservable<EventDataModel> OnCreateObservable => _onCreateSubject.AsObservable();
        public IObservable<EventDataModel> OnStatusUpdateObservable() => _onStatusUpdateSubject.AsObservable();
    }
}