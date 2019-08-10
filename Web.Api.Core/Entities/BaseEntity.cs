using System;
using System.Collections.Generic;
using Web.Api.Core.Events;

namespace Web.Api.Core.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public List<DomainEvent> Events = new List<DomainEvent>();
    }
}
