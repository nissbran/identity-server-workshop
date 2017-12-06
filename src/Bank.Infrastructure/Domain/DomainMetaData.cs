namespace Bank.Infrastructure.Domain
{
    using System;

    public class DomainMetadata
    {
        public Guid CommitId { get; set; }

        public string Schema { get; set; }

        public DateTimeOffset Created { get; set; }
    }
}