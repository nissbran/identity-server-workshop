namespace Bank.Cards.Domain.Card.Events
{
    using System;
    using Infrastructure.Domain;

    [EventName("CardConnectedToAccount")]
    public class CardConnectedToAccountEvent : CardDomainEvent
    {
        public Guid AccountId { get; set; }
    }
}