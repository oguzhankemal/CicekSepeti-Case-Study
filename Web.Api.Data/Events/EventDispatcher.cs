using Autofac;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Api.Core.Events;
using Web.Api.Core.Interfaces;

namespace Web.Api.Infrastructure.Events
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IComponentContext _container;

        public EventDispatcher(IComponentContext container)
        {
            _container = container;
        }

        public void Dispatch(DomainEvent dEvent)
        {
            Type handlerType = typeof(IHandle<>).MakeGenericType(dEvent.GetType());
            Type wrapperType = typeof(DomainEventHandler<>).MakeGenericType(dEvent.GetType());
            IEnumerable handlers = (IEnumerable)_container.Resolve(typeof(IEnumerable<>).MakeGenericType(handlerType));
            IEnumerable<EventHandler> wrappedHandlers = handlers.Cast<object>()
                .Select(handler => (EventHandler)Activator.CreateInstance(wrapperType, handler));

            foreach (EventHandler handler in wrappedHandlers)
            {
                handler.Handle(dEvent);
            }
        }

        private abstract class EventHandler
        {
            public abstract void Handle(DomainEvent domainEvent);
        }

        private class DomainEventHandler<T> : EventHandler
            where T : DomainEvent
        {
            private readonly IHandle<T> _handler;

            public DomainEventHandler(IHandle<T> handler)
            {
                _handler = handler;
            }

            public override void Handle(DomainEvent domainEvent)
            {
                _handler.Handle((T)domainEvent);
            }
        }
    }
}
