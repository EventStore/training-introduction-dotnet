using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Infrastructure.ES;
using EventStore.Client;

namespace Application.Infrastructure.Projections
{

    namespace Scheduling.Domain.Infrastructure.Projections
    {
        public class SubscriptionManager
        {
            readonly string _name;
            readonly StreamName _streamName;
            readonly EventStoreClient _client;
            readonly ISubscription[] _subscriptions;
            StreamSubscription? _subscription;
            readonly bool _isAllStream;

            public SubscriptionManager(
                EventStoreClient client,
                string name,
                StreamName streamName,
                params ISubscription[] subscriptions
            )
            {
                _client = client;
                _name = name;
                _streamName = streamName;
                _subscriptions = subscriptions;
                _isAllStream = streamName.IsAllStream;
            }

            public async Task Start()
            {
                _subscription = _isAllStream
                    ? await _client.SubscribeToAllAsync(
                        FromAll.Start,
                        EventAppeared)
                    : await _client.SubscribeToStreamAsync(
                        _streamName,
                        FromStream.Start,
                        EventAppeared
                    );
            }

            async Task EventAppeared(StreamSubscription _, ResolvedEvent resolvedEvent, CancellationToken c)
            {
                if (resolvedEvent.Event.EventType.StartsWith("$")) return;

                var @event = resolvedEvent.Deserialize()!;
                await Task.WhenAll(
                    _subscriptions.Select(x => x.Project(@event))
                );
            }
        }
    }
}
