namespace Bank.Cards.Domain.Account
{
    using System;
    using Infrastructure.EventStore;

    public class AccountStreamId : StreamId
    {
        public Guid Id { get; }

        public AccountStreamId(Guid id)
        {
            Id = id;
        }

        public override string ToStreamName()
        {
            return $"Account-{Id}";
        }
    }
}