namespace Bank.Cards.Domain.Card
{
    using System;
    using Events;
    using Infrastructure.Domain;
    using State;

    public abstract class CardAggregateRoot : AggregateRoot
    {
        public string Id { get; protected set; }

        public CardState State { get; protected set; }

        public void AddEvent(CardDomainEvent domainEvent)
        {
            ApplyEvent(domainEvent);

            UncommittedEvents.Add(domainEvent);

            Version++;
        }

        protected abstract void ApplyEvent(CardDomainEvent domainEvent);

        protected void ApplyCardEvent(CardDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case CardDetailsSetEvent cardDetailsSetEvent:
                    OnCardDetailsSet(cardDetailsSetEvent);
                    break;
                case CardConnectedToAccountEvent cardConnectedToAccountEvent:
                    OnCardConnectedToAccountEvent(cardConnectedToAccountEvent);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void OnCardDetailsSet(CardDetailsSetEvent cardDetailsSetEvent)
        {
            State.NameOnCard = cardDetailsSetEvent.NameOnCard;
        }

        private void OnCardConnectedToAccountEvent(CardConnectedToAccountEvent cardConnectedToAccountEvent)
        {
            State.AccountId = cardConnectedToAccountEvent.AccountId;
        }
    }
}