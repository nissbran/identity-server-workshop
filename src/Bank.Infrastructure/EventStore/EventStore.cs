namespace Bank.Infrastructure.EventStore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Domain;
    using global::EventStore.ClientAPI;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class EventStore : IEventStore
    {
        private const long MinStreamVersion = 0;
        private const long MaxStreamVersion = long.MaxValue;
        private const int ReadBatchSize = 500;
        private const int WriteBatchSize = 500;

        private readonly IEventStoreConnection _connection;
        private readonly Dictionary<string, IEventSchema> _eventSchemas = new Dictionary<string, IEventSchema>();
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public EventStore(IEventStoreConnection connection, IEnumerable<IEventSchema> eventSchemas)
        {
            _connection = connection;

            foreach (var schema in eventSchemas)
            {
                _eventSchemas.Add(schema.Name, schema);
            }

            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public async Task<List<IDomainEvent>> GetEventsByStreamId(StreamId streamId)
        {
            var streamEvents = new List<ResolvedEvent>();

            StreamEventsSlice currentSlice;
            var nextSliceStart = MinStreamVersion;
            do
            {
                long nextSliceSize = ReadBatchSize;
                var nextSliceEnd = nextSliceStart + nextSliceSize;
                if (nextSliceEnd > MaxStreamVersion)
                    nextSliceSize = MaxStreamVersion - nextSliceStart;

                currentSlice = await _connection.ReadStreamEventsForwardAsync(streamId.ToStreamName(), nextSliceStart, (int)nextSliceSize, streamId.ResolveLinks);

                nextSliceStart = currentSlice.NextEventNumber;

                streamEvents.AddRange(currentSlice.Events);
            } while (!currentSlice.IsEndOfStream && currentSlice.NextEventNumber < MaxStreamVersion);

            return streamEvents.Select(ConvertEventDataToDomainEvent).ToList();
        }

        public async Task<StreamWriteResult> SaveEvents(StreamId streamId, long streamVersion, List<IDomainEvent> events)
        {
            if (events.Any() == false)
                return new StreamWriteResult(-1);

            var commitId = Guid.NewGuid();

            var expectedVersion = streamVersion == 0 ? ExpectedVersion.NoStream : streamVersion - 1;
            var eventsToSave = events.Select(domainEvent => ToEventData(commitId, domainEvent)).ToList();

            if (eventsToSave.Count < WriteBatchSize)
            {
                var result = await _connection.AppendToStreamAsync(streamId.ToString(), expectedVersion, eventsToSave);

                return new StreamWriteResult(result.NextExpectedVersion);
            }

            using (var transaction = await _connection.StartTransactionAsync(streamId.ToString(), expectedVersion))
            {
                var position = 0;
                while (position < eventsToSave.Count)
                {
                    var pageEvents = eventsToSave.Skip(position).Take(WriteBatchSize);
                    await transaction.WriteAsync(pageEvents);
                    position += WriteBatchSize;
                }

                var result = await transaction.CommitAsync();

                return new StreamWriteResult(result.NextExpectedVersion);
            }
        }
        
        private IDomainEvent ConvertEventDataToDomainEvent(ResolvedEvent resolvedEvent)
        {
            var metadataString = Encoding.UTF8.GetString(resolvedEvent.Event.Metadata);
            var eventString = Encoding.UTF8.GetString(resolvedEvent.Event.Data);

            var metadata = JsonConvert.DeserializeObject<DomainMetadata>(metadataString, _jsonSerializerSettings);

            _eventSchemas.TryGetValue(metadata.Schema, out var schema);

            var eventType = schema.GetDomainEventType(resolvedEvent.Event.EventType);

            return (IDomainEvent)JsonConvert.DeserializeObject(eventString, eventType, _jsonSerializerSettings);
        }

        private EventData ToEventData(Guid commitId, IDomainEvent domainEvent)
        {
            _eventSchemas.TryGetValue(domainEvent.Schema, out var schema);

            var definition = schema.GetEventDefinition(domainEvent);

            var dataJson = JsonConvert.SerializeObject(domainEvent, _jsonSerializerSettings);
            var metadataJson = JsonConvert.SerializeObject(new DomainMetadata
            {
                CommitId = commitId,
                Schema = domainEvent.Schema,
                Created = DateTimeOffset.UtcNow
            }, _jsonSerializerSettings);

            var data = Encoding.UTF8.GetBytes(dataJson);
            var metadata = Encoding.UTF8.GetBytes(metadataJson);

            return new EventData(Guid.NewGuid(), definition.EventName, true, data, metadata);
        }
    }
}