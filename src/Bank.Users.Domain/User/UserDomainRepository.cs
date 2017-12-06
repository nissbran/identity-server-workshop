namespace Bank.Users.Domain.User
{
    using System.Threading.Tasks;
    using Infrastructure.EventStore;

    public class UserDomainRepository : IUserDomainRepository
    {
        private readonly IEventStore _eventStore;

        public UserDomainRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<UserAggregateRoot> GetUserByUsername(string username)
        {
            var domainEvents = await _eventStore.GetEventsByStreamId(new UserStreamId(username));

            if (domainEvents.Count == 0)
                return null;

            return new UserAggregateRoot(domainEvents);
        }

        public async Task SaveUser(UserAggregateRoot user)
        {
            var domainEvents = user.UncommittedEvents;

            var streamVersion = user.Version - domainEvents.Count;

            var result = await _eventStore.SaveEvents(new UserStreamId(user.Username), streamVersion, domainEvents);
        }
    }
}