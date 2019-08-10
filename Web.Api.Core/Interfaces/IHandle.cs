using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Events;

namespace Web.Api.Core.Interfaces
{
    public interface IHandle<T> where T : DomainEvent
    {
        void Handle(T dEvent);
    }
}
