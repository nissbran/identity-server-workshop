namespace Bank.Cards.Transactional.Api.Models.Card
{
    public class CardPurchaseResponse
    {
        public decimal Balance { get; set; }

        public CardPurchaseResponse(decimal balance)
        {
            Balance = balance;
        }
    }
}