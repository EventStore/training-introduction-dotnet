using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scheduling.Domain.Infrastructure
{
    public interface IEventStore
    {
        Task AppendEvents(
            string streamName,
            long version,
            params object[] events
        );

        Task<IEnumerable<object>> LoadEvents(string stream, int? version = null);
    }
}
