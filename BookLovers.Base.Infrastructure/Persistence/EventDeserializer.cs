using System.Collections.Generic;
using BookLovers.Base.Domain.Events;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Serialization;
using Newtonsoft.Json;

namespace BookLovers.Base.Infrastructure.Persistence
{
    public static class EventDeserializer
    {
        public static IList<IEvent> DeserializeToDomainEvents(
            IEnumerable<IEventEntity> serializedEvents)
        {
            var eventList = new List<IEvent>();

            foreach (var serializedEvent in serializedEvents)
            {
                var instance = ReflectionHelper.CreateInstance(serializedEvent.Assembly, serializedEvent.Type);

                var @event = JsonConvert.DeserializeObject(
                    serializedEvent.Data,
                    instance.GetType(),
                    SerializerSettings.GetSerializerSettings());

                eventList.Add((IEvent) @event);
            }

            return eventList;
        }
    }
}