namespace Bank.Infrastructure.EventStore
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain;

    public interface IEventStore
    {
        Task<List<IDomainEvent>> GetEventsByStreamId(StreamId streamId);

        Task<StreamWriteResult> SaveEvents(StreamId streamId, long streamVersion, List<IDomainEvent> events);
    }
}