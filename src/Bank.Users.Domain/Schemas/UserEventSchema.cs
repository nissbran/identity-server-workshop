namespace Bank.Users.Domain.Schemas
{
    using Infrastructure.EventStore;
    using User.Events;

    public class UserEventSchema : EventSchema<UserDomainEvent>
    {
        public const string SchemaName = "User";

        public override string Name => SchemaName;
    }
}