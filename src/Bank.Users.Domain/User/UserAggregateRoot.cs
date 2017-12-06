namespace Bank.Users.Domain.User
{
    using System.Collections.Generic;
    using Events;
    using Infrastructure.Domain;
    using State;

    public class UserAggregateRoot : AggregateRoot
    {
        public string Username { get; private set; }

        public UserState State { get; }

        private UserAggregateRoot()
        {
            State = new UserState();
        }

        public UserAggregateRoot(string username) : this()
        {
            AddEvent(new UserCreatedEvent
            {
                Username = username
            });
        }

        public UserAggregateRoot(IEnumerable<IDomainEvent> historicEvents) : this()
        {
            foreach (var historicEvent in historicEvents)
            {
                ApplyEvent((UserDomainEvent)historicEvent);

                Version++;
            }
        }

        public void AddEvent(UserDomainEvent domainEvent)
        {
            ApplyEvent(domainEvent);

            UncommittedEvents.Add(domainEvent);

            Version++;
        }

        private void ApplyEvent(UserDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case UserCreatedEvent userCreatedEvent:
                    OnUserCreated(userCreatedEvent);
                    break;
            }
        }

        private void OnUserCreated(UserCreatedEvent userCreatedEvent)
        {
            Username = userCreatedEvent.Username;
        }
    }
}