namespace Bank.Cards.Domain.Card
{
    using System.Threading.Tasks;

    public interface ICardDomainRepository
    {
        Task<CardAggregateRoot> GetCardByHashedPan(string hashedPan);

        Task SaveCard(CardAggregateRoot card);
    }
}