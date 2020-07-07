using System;
using Application.Domain.Commands;
using Scheduling.Domain.Infrastructure;
using Scheduling.Domain.Infrastructure.Commands;

namespace Application.Domain.WriteModel.Commands
{
    public class Handlers : CommandHandler
    {
        public Handlers(IAggregateStore aggregateStore)
        {
            Register<Schedule>(async s =>
            {
                var aggregate = await aggregateStore.Load<SlotAggregate>(s.Id);
                aggregate.Schedule(s.Id, s.StartTime, s.Duration);
                await aggregateStore.Save(aggregate);
            });
            Register<Book>(async b =>
            {
                var aggregate = await aggregateStore.Load<SlotAggregate>(b.Id);
                aggregate.Book(b.PatientId);
                await aggregateStore.Save(aggregate);
            });
            Register<Cancel>(async c =>
            {
                var aggregate = await aggregateStore.Load<SlotAggregate>(c.Id);
                aggregate.Cancel(c.Reason, c.CancellationTime);
                await aggregateStore.Save(aggregate);
            });
        }
    }
}