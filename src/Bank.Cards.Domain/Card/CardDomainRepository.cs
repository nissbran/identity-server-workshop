namespace Bank.Cards.Domain.Card
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Credit;
    using Credit.Events;
    using Debit;
    using Debit.Events;
    using Infrastructure.EventStore;

    public class CardDomainRepository : ICardDomainRepository
    {
        private readonly IEventStore _eventStore;

        public CardDomainRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<CardAggregateRoot> GetCardByHashedPan(string hashedPan)
        {
            var domainEvents = await _eventStore.GetEventsByStreamId(new CardStreamId(hashedPan));

            if (domainEvents.Count == 0)
                return null;

            var firstEvent = domainEvents.First();
            switch (firstEvent)
            {
                case DebitCardCreatedEvent _:
                    return new DebitCardAggregateRoot(domainEvents);
                case CreditCardCreatedEvent _:
                    return new CreditCardAggregateRoot(domainEvents);
                default:
                    throw new NotImplementedException();
            }
        }

        public async Task SaveCard(CardAggregateRoot card)
        {
            var domainEvents = card.UncommittedEvents;

            var streamVersion = card.Version - domainEvents.Count;

            var result = await _eventStore.SaveEvents(new CardStreamId(card.Id), streamVersion, domainEvents);
        }
    }
}