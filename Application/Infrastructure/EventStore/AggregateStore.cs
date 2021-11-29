using System;
using System.Linq;
using System.Threading.Tasks;
using Scheduling.Domain.Infrastructure;

namespace Application.Infrastructure.ES;

public class EsAggregateStore : IAggregateStore
{
    readonly IEventStore _store;

    private readonly int _threshold;

    public EsAggregateStore(IEventStore store)
    {
        _store = store;
    }

    public async Task Save<T>(T aggregate) where T : AggregateRoot
    {
        if (aggregate == null)
            throw new ArgumentNullException(nameof(aggregate));

        var streamName = GetStreamName(aggregate);
        var changes = aggregate.GetChanges().ToArray();

        await _store.AppendEvents(streamName, aggregate.Version, changes);
        aggregate.ClearChanges();
    }

    public async Task<T> Load<T>(string aggregateId)
        where T : AggregateRoot
    {
        if (aggregateId == null)
            throw new ArgumentNullException(nameof(aggregateId));

        var streamName = GetStreamName<T>(aggregateId);
        var aggregate = (T) Activator.CreateInstance(typeof(T), true);

        aggregate.Id = aggregateId;

        var events = await _store.LoadEvents(streamName);

        aggregate.Load(events);
        aggregate.ClearChanges();

        return aggregate;
    }

    static string GetStreamName<T>(string aggregateId)
        where T : AggregateRoot
        => $"{typeof(T).Name}-{aggregateId}";

    static string GetStreamName<T>(T aggregate)
        where T : AggregateRoot
        => $"{typeof(T).Name}-{aggregate.Id:N}";
}