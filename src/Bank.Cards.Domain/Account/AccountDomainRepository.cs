namespace Bank.Cards.Domain.Account
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Credit;
    using Credit.Events;
    using Debit;
    using Debit.Events;
    using Infrastructure.EventStore;

    public class AccountDomainRepository : IAccountDomainRepository
    {
        private readonly IEventStore _eventStore;

        public AccountDomainRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<AccountAggregateRoot> GetAccountById(Guid cardId)
        {
            var domainEvents = await _eventStore.GetEventsByStreamId(new AccountStreamId(cardId));

            if (domainEvents.Count == 0)
                return null;

            var firstEvent = domainEvents.First();
            switch (firstEvent)
            {
                case DebitAccountCreatedEvent _:
                    return new DebitAccountAggregateRoot(domainEvents);
                case CreditAccountCreatedEvent _:
                    return new CreditAccountAggregateRoot(domainEvents);
                default:
                    throw new NotImplementedException();
            }
        }

        public async Task SaveAccount(AccountAggregateRoot account)
        {
            var domainEvents = account.UncommittedEvents;
            
            var streamVersion = account.Version - domainEvents.Count;

            var result = await _eventStore.SaveEvents(new AccountStreamId(account.Id), streamVersion, domainEvents);
        }
    }
}