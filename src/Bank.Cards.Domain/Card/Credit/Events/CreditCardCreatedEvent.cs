namespace Bank.Cards.Domain.Card.Credit.Events
{
    using Card.Events;
    using Infrastructure.Domain;

    [EventName("CreditCardCreated")]
    public class CreditCardCreatedEvent : CardCreatedEvent
    {
        
    }
}