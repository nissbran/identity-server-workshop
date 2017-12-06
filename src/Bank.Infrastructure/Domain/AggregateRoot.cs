namespace Bank.Infrastructure.Domain
{
    using System.Collections.Generic;

    public abstract class AggregateRoot : IAggregateRoot
    {
        public int Version { get; set; }

        public List<IDomainEvent> UncommittedEvents { get; }

        protected AggregateRoot()
        {
            UncommittedEvents = new List<IDomainEvent>();
        }

        public void CommitEvents()
        {
            UncommittedEvents.Clear();
        }
    }
}