using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.Client;
using Scheduling.Domain.Infrastructure;

namespace Application.Infrastructure.ES
{
    public class EsEventStore : IEventStore
    {
        private readonly EventStoreClient _client;

        public EsEventStore(EventStoreClient client)
        {
            _client = client;
        }

        public Task AppendEvents(string streamName, long version, params object[] events)
        {
            if (events == null || !events.Any())
            {
                return Task.CompletedTask;
            }

            var preparedEvents = events.Select(e => e.Serialize()).ToList();

            if (version == -1)
            {
                return _client.AppendToStreamAsync(ToStreamName(streamName), StreamState.NoStream, preparedEvents);
            }

            return _client.AppendToStreamAsync(ToStreamName(streamName), Convert.ToUInt64(version), preparedEvents);
        }

        public async Task<IEnumerable<object>> LoadEvents(string streamName)
        {
            var response = _client
                .ReadStreamAsync(Direction.Forwards, ToStreamName(streamName), StreamPosition.Start);

            var readState = await response.ReadState;
            if (readState == ReadState.StreamNotFound)
            {
                return new List<object>();
            }

            var events = (await response.ToListAsync())
                .Select(e => e.Deserialize())
                .ToList();
            return events;
        }

        private string ToStreamName(string streamName) =>
            streamName.Replace(" ", "");
    }
}
