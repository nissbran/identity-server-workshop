namespace Bank.Cards.Domain.Statistics
{
    using Infrastructure.EventStore;

    public class StatisticsAllCardsStreamId : StreamId
    {
        public override bool ResolveLinks { get; } = true;

        public override string ToStreamName()
        {
            return "$ce-Card";
        }
    }
}