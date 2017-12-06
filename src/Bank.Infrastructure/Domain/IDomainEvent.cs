namespace Bank.Infrastructure.Domain
{
    public interface IDomainEvent
    {
        string Schema { get; }

        DomainMetadata Metadata { get; set; }
    }
}