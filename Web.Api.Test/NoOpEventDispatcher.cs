using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Events;
using Web.Api.Core.Interfaces;

namespace Web.Api.Test
{
    public class NoOpEventDispatcher : IEventDispatcher
    {
        public void Dispatch(DomainEvent domainEvent) { }
    }
}
