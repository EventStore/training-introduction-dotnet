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

            var preparedEvents = events.Select(e => e.Serialize(
                Uuid.NewUuid()
            )).ToList();

            if (version == -1)
            {
                return _client.AppendToStreamAsync(streamName, StreamState.NoStream, preparedEvents);
            }

            return _client.AppendToStreamAsync(streamName, Convert.ToUInt64(version), preparedEvents);
        }

        public async Task<IEnumerable<object>> LoadEvents(string stream)
        {
            var response = _client
                .ReadStreamAsync(Direction.Forwards, stream, StreamPosition.Start);

            if (await response.ReadState == ReadState.StreamNotFound)
            {
                return new List<object>();
            }

            return await response
                .Select(e => e.Deserialize())
                .ToListAsync();
        }
    }
}
