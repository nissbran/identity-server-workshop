namespace Bank.Cards.Transactional.Api.Models.Card
{
    public class CardAuthorizeRequest
    {
        public string Pan { get; set; }

        public decimal Amount { get; set; }
    }
}