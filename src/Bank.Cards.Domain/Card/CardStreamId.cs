namespace Bank.Cards.Domain.Card
{
    using Infrastructure.EventStore;

    public class CardStreamId : StreamId
    {
        public string HashedPan { get; }

        public CardStreamId(string hashedPan)
        {
            HashedPan = hashedPan;
        }

        public override string ToStreamName()
        {
            return $"Card-{HashedPan}";
        }
    }
}