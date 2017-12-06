namespace Bank.Cards.Domain.Account.State
{
    public abstract class AccountState
    {
        public decimal Balance { get; internal set; }

        public long IssuerId { get; internal set; }
    }
}