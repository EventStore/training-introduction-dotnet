using System.Threading.Tasks;
using Application.Infrastructure;

namespace Scheduling.Domain.Infrastructure;

public interface IAggregateStore
{
    Task Save<T>(T aggregate) where T : AggregateRoot;

    Task<T> Load<T>(string aggregateId) where T : AggregateRoot;
}