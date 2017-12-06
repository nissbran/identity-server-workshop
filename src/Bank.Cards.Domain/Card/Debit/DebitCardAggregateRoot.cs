namespace Bank.Cards.Domain.Card.Debit
{
    using System.Collections.Generic;
    using Card.Events;
    using Events;
    using Infrastructure.Domain;
    using State;

    public sealed class DebitCardAggregateRoot : CardAggregateRoot
    {
        private DebitCardAggregateRoot()
        {
            State = new DebitCardState();
        }

        public DebitCardAggregateRoot(string hashedPan) : this()
        {
            Id = hashedPan;
        }

        public DebitCardAggregateRoot(IEnumerable<IDomainEvent> historicEvents) : this()
        {
            foreach (var historicEvent in historicEvents)
            {
                ApplyEvent((CardDomainEvent)historicEvent);

                Version++;
            }
        }

        protected override void ApplyEvent(CardDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case DebitCardCreatedEvent createdEvent:
                    OnCardCreated(createdEvent);
                    break;
                default:
                    ApplyCardEvent(domainEvent);
                    break;
            }
        }

        private void OnCardCreated(CardCreatedEvent cardAccountCreated)
        {
            Id = cardAccountCreated.HashedPan;
        }
    }
}