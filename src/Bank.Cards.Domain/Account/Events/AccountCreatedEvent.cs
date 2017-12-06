namespace Bank.Cards.Domain.Account.Events
{
    public abstract class AccountCreatedEvent : AccountDomainEvent
    {
        public string CurrencyIso { get; set; }
    }
}