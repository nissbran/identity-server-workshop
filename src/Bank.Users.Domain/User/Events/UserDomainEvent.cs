namespace Bank.Users.Domain.User.Events
{
    using Infrastructure.Domain;
    using Newtonsoft.Json;
    using Schemas;

    public abstract class UserDomainEvent : IDomainEvent
    {
        [JsonIgnore]
        public string Schema => UserEventSchema.SchemaName;

        [JsonIgnore]
        public DomainMetadata Metadata { get; set; }
    }
}