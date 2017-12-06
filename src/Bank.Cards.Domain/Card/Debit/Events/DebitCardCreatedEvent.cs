namespace Bank.Cards.Domain.Card.Debit.Events
{
    using Card.Events;
    using Infrastructure.Domain;

    [EventName("DebitCardCreated")]
    public class DebitCardCreatedEvent : CardCreatedEvent
    {
        
    }
}