using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.EventSourcing;
using EventStore.Client;
using Scheduling.Domain.Infrastructure;

namespace Application.Infrastructure.ES
{
    public class EsEventStore : IEventStore
    {
        private readonly EventStoreClient _client;
        private readonly string _tenantPrefix;

        public EsEventStore(EventStoreClient client, string tenantPrefix)
        {
            _client = client;
            _tenantPrefix = $"[{tenantPrefix}]";
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

            // var preparedEvents = events.Select(
            //     @event => new EventData(
            //         Uuid.NewUuid(),
            //         TypeMapper.GetTypeName(@event.GetType()),
            //         Serialize(@event),
            //         Serialize(new EventMetadata
            //         {
            //             ClrType = @event.GetType().FullName,
            //             CausationId = metadata.CausationId.Value.ToString(),
            //             CorrelationId = metadata.CorrelationId.Value.ToString(),
            //             Replayed = metadata.Replayed
            //         })
            //     )
            // ).ToList();

            if (version == -1)
            {
                return _client.AppendToStreamAsync(_tenantPrefix + streamName, StreamState.NoStream, preparedEvents);
            }

            return _client.AppendToStreamAsync(_tenantPrefix + streamName, Convert.ToUInt64(version), preparedEvents);
        }

        public async Task<IEnumerable<object>> LoadEvents(string stream, int? version = null)
        {
            EventStoreClient.ReadStreamResult response;

            if (version == null || version == -1)
            {
                response = _client
                    .ReadStreamAsync(Direction.Forwards, _tenantPrefix + stream, StreamPosition.Start);
            }
            else
            {
                response = _client
                    .ReadStreamAsync(Direction.Forwards, _tenantPrefix + stream, Convert.ToUInt64(version));
            }


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