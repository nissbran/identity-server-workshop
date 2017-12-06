namespace Bank.Infrastructure.Domain
{
    using System.Collections.Generic;

    public interface IAggregateRoot
    {
        int Version { get; set; }
        
        List<IDomainEvent> UncommittedEvents { get; }

        void CommitEvents();
    }
}