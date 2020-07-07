using System.Text;
using EventStore.Client;
using Newtonsoft.Json;

namespace Application.Infrastructure.ES
{
    public static class EsEventSerde
    {
        public static object Deserialize(this ResolvedEvent resolvedEvent)
        {
            return JsonConvert
                .DeserializeObject(Encoding.UTF8.GetString(resolvedEvent.Event.Data.ToArray()),
                    new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects});
        }

        public static EventData Serialize(this object data)
        {
            return new EventData(
                Uuid.NewUuid(),
                data.GetType().Name,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data,
                    new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects})));
        }
    }
}
