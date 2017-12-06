namespace Bank.Cards.Domain.Card.Events
{
    using System;

    public abstract class CardCreatedEvent : CardDomainEvent
    {
        public string EncryptedPan { get; set; }

        public string HashedPan { get; set; }

        public DateTimeOffset ExpiryDate { get; set; }
    }
}