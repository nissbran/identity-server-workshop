namespace Bank.Cards.Domain.Account.Credit.State
{
    using Account.State;

    public sealed class CreditAccountState : AccountState
    {
        public decimal CreditLimit { get; internal set; }
    }
}