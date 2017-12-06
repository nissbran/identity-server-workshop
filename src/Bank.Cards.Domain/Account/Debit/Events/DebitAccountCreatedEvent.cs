namespace Bank.Cards.Domain.Account.Debit.Events
{
    using Account.Events;
    using Infrastructure.Domain;

    [EventName("DebitAccountCreated")]
    public class DebitAccountCreatedEvent : AccountCreatedEvent
    {
        
    }
}