namespace Bank.Cards.Domain.Statistics
{
    using Infrastructure.EventStore;

    public class StatisticsAllAccountStreamId : StreamId
    {
        public override bool ResolveLinks { get; } = true;

        public override string ToStreamName()
        {
            return "$ce-Account";
        }
    }
}