using System.Collections.Generic;

namespace Application.EventSourcing;

public abstract class AggregateState
{
    public abstract void Apply(List<IEvent> events);
}