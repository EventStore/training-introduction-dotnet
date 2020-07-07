using System.Text;
using EventStore.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scheduling.Domain.Infrastructure;

namespace Application.Infrastructure.ES
{
    public static class EsEventSerde
    {
        public static object Deserialize(this ResolvedEvent resolvedEvent)
        {
            var dataToType = TypeMapper.GetDataToType(resolvedEvent.Event.EventType);
            var jsonData = Encoding.UTF8.GetString(resolvedEvent.Event.Data.ToArray());
            var data = JObject.Parse(jsonData);
            return dataToType(data);
        }

        public static EventData Serialize(this object @event, Uuid uuid)
        {
            var typeToData = TypeMapper.GetTypeToData(@event.GetType());
            var (name, jObject) = typeToData(@event);
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jObject));

            return new EventData(
                uuid,
                name,
                data,
                Serialize(new EventMetadata
                {
                    ClrType = @event.GetType().FullName
                })
            );
        }

        private static byte[] Serialize(this object data) =>
            Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

        public static T Deserialize<T>(this ResolvedEvent resolvedEvent)
        {
            var jsonData = Encoding.UTF8.GetString(resolvedEvent.Event.Data.ToArray());
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
    }
}