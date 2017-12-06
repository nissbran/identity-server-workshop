namespace Bank.Cards.Domain.Account.Events
{
    using System;
    using Infrastructure.Domain;
    using Newtonsoft.Json;
    using Schemas;

    public abstract class AccountDomainEvent : IDomainEvent
    {
        public Guid AggregateRootId { get; set; }

        [JsonIgnore]
        public string Schema => AccountSchema.SchemaName;

        [JsonIgnore]
        public DomainMetadata Metadata { get; set; }
    }
}