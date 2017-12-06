namespace Bank.Cards.Domain.Schemas
{
    using Card.Events;
    using Infrastructure.EventStore;

    public class CardSchema : EventSchema<CardDomainEvent>
    {
        public const string SchemaName = "Card";

        public override string Name => SchemaName;
    }
}