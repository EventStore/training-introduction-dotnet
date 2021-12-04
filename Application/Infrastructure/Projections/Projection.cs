using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Infrastructure.Projections;

public class Projection
{
    readonly List<EventHandler> _handlers = new();

    protected void When<T>(Func<T, Task> when)
    {
        _handlers.Add(new EventHandler(typeof(T), async e => await when((T)e)));
    }

    public async Task Handle(Type eventType, object e)
    {
        var handlers = _handlers
            .Where(h => h.EventType == eventType)
            .ToList();

        foreach (var handler in handlers)
        {
            await handler.Handler(e);
        }
    }

    public bool CanHandle(Type eventType)
    {
        return _handlers.Any(h => h.EventType == eventType);
    }

    public class EventHandler
    {
        public Type EventType { get; }
        
        public Func<object, Task> Handler { get; }
        
        public EventHandler(Type eventType, Func<object, Task> handler)
        {
            EventType = eventType;
            Handler = handler;
        }
    }
}