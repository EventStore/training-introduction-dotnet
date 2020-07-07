using System.Threading.Tasks;
using Application.EventSourcing;
using Application.Infrastructure;
using Scheduling.Domain.Infrastructure;

namespace Scheduling.Test
{
    public class FakeAggregateStore : IAggregateStore
    {
        private readonly AggregateRoot _aggregateRoot;

        public FakeAggregateStore(AggregateRoot aggregateRoot)
        {
            _aggregateRoot = aggregateRoot;
        }

        public Task Save<T>(T aggregate) where T : AggregateRoot
        {
            return Task.CompletedTask;
        }

        public Task<T> Load<T>(string aggregateId) where T : AggregateRoot
        {
            return Task.FromResult((T) _aggregateRoot);
        }
    }
}
