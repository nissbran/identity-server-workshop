namespace Bank.Cards.Domain.Account.Credit.Events
{
    using Account.Events;
    using Infrastructure.Domain;

    [EventName("CreditAccountCreated")]
    public class CreditAccountCreatedEvent : AccountCreatedEvent
    {
        
    }
}