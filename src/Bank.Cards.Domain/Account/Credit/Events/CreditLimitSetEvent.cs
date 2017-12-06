namespace Bank.Cards.Domain.Account.Credit.Events
{
    using Account.Events;
    using Infrastructure.Domain;

    [EventName("CreditLimitSet")]
    public class CreditLimitSetEvent : AccountDomainEvent
    {
        public decimal CreditLimit { get; set; }
    }
}