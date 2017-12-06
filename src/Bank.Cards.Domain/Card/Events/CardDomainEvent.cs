namespace Bank.Cards.Domain.Card.Events
{
    using Infrastructure.Domain;
    using Newtonsoft.Json;
    using Schemas;

    public abstract class CardDomainEvent : IDomainEvent
    {
        [JsonIgnore]
        public string Schema => CardSchema.SchemaName;

        [JsonIgnore]
        public DomainMetadata Metadata { get; set; }
    }
}