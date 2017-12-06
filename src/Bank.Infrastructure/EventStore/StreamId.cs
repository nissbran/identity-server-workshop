namespace Bank.Infrastructure.EventStore
{
    public abstract class StreamId
    {
        public abstract string ToStreamName();

        public virtual bool ResolveLinks { get; } = false;

        public override string ToString()
        {
            return ToStreamName();
        }
    }
}