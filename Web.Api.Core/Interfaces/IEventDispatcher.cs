using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Events;

namespace Web.Api.Core.Interfaces
{
    public interface IEventDispatcher
    {
        void Dispatch(DomainEvent bEvent);
    }
}
