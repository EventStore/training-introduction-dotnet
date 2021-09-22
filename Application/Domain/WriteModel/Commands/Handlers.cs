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

            // Register<Book>(async s =>
            // {
            //
            // });
            //
            // Register<Cancel>(async s =>
            // {
            //
            // });
        }
    }
}
