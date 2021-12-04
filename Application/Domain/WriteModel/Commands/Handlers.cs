using Scheduling.Domain.Infrastructure;
using Scheduling.Domain.Infrastructure.Commands;

namespace Application.Domain.WriteModel.Commands;

public class Handlers : CommandHandler
{
    public Handlers(IAggregateStore aggregateStore)
    {
        Register<Schedule>(async s =>
        {
            var (id, startTime, timeSpan) = s;
            var aggregate = await aggregateStore.Load<SlotAggregate>(id);
            aggregate.Schedule(id, startTime, timeSpan);
            await aggregateStore.Save(aggregate);
        });
        Register<Book>(async b =>
        {
            var (id, patientId) = b;
            var aggregate = await aggregateStore.Load<SlotAggregate>(id);
            aggregate.Book(patientId);
            await aggregateStore.Save(aggregate);
        });
        Register<Cancel>(async c =>
        {
            var (id, reason, cancellationTime) = c;
            var aggregate = await aggregateStore.Load<SlotAggregate>(id);
            aggregate.Cancel(reason, cancellationTime);
            await aggregateStore.Save(aggregate);
        });
    }
}