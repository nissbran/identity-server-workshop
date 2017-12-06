namespace Bank.Cards.Domain.Card.Credit
{
    using System;
    using System.Collections.Generic;
    using Card.Events;
    using Events;
    using Infrastructure.Domain;
    using State;

    public sealed class CreditCardAggregateRoot : CardAggregateRoot
    {
        public CreditCardState CreditCardState => State as CreditCardState;

        private CreditCardAggregateRoot()
        {
            State = new CreditCardState();
        }

        public CreditCardAggregateRoot(string hashedPan) : this()
        {
            Id = hashedPan;
        }

        public CreditCardAggregateRoot(IEnumerable<IDomainEvent> historicEvents) : this()
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
                case CreditCardCreatedEvent cardCreatedEvent:
                    OnCardCreated(cardCreatedEvent);
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