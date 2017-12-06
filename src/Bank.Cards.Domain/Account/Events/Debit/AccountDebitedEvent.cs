namespace Bank.Cards.Domain.Account.Events.Debit
{
    using Infrastructure.Domain;

    [EventName("AccountDebited")]
    public class AccountDebitedEvent : AccountDomainEvent
    {
        public decimal Amount { get; set; }

        public string CreatedBy { get; set; }
    }
}