namespace Bank.Users.Domain.User.Events
{
    using Infrastructure.Domain;

    [EventName("UserCreated")]
    public class UserCreatedEvent : UserDomainEvent
    {
        public string Username { get; set; }
    }
}