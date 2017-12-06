namespace Bank.Cards.Domain.Account.Events.Credit
{
    using Infrastructure.Domain;

    [EventName("AccountCredited")]
    public class AccountCreditedEvent : AccountDomainEvent
    {
        public decimal Amount { get; set; }
    }
}