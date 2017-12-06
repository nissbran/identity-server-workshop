namespace Bank.Cards.Admin.Api.Models.Accounts
{
    using System;
    using Domain.Account;

    public class GetCardAccountResponse
    {
        public Guid Id { get; }

        public decimal Balance { get; }

        public IssuerReponseItem Issuer { get; }

        public GetCardAccountResponse(AccountAggregateRoot accountAggregateRoot)
        {
            Id = accountAggregateRoot.Id;
            Balance = accountAggregateRoot.State.Balance;
            Issuer = new IssuerReponseItem(accountAggregateRoot.State.IssuerId);
        }
    }
}