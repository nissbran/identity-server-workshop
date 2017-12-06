namespace Bank.Users.Domain.User.Events
{
    using Infrastructure.Domain;

    [EventName("UserConnectedToIssuer")]
    public class UserConnectedToIssuerEvent : UserDomainEvent
    {
        public long IssuerId { get; set; }
    }
}