namespace Bank.Cards.Admin.Api.Models.Cards
{
    using System;
    using Domain.Card;

    public class GetCardResponse
    {
        public string NameOnCard { get; }

        public Guid AccountId { get; }

        public GetCardResponse(CardAggregateRoot card)
        {
            NameOnCard = card.State.NameOnCard;
            AccountId = card.State.AccountId;
        }
    }
}