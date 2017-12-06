namespace Bank.Cards.Domain.Account.Credit
{
    using System;
    using System.Collections.Generic;
    using Account.Events;
    using Events;
    using Infrastructure.Domain;
    using State;

    public sealed class CreditAccountAggregateRoot : AccountAggregateRoot
    {
        public CreditAccountState CreditAccountState => State as CreditAccountState;

        private CreditAccountAggregateRoot()
        {
            State = new CreditAccountState();
        }

        public CreditAccountAggregateRoot(Guid id) : this()
        {
            Id = id;

            AddEvent(new CreditAccountCreatedEvent());
        }

        public CreditAccountAggregateRoot(IEnumerable<IDomainEvent> historicEvents) : this()
        {
            foreach (var historicEvent in historicEvents)
            {
                ApplyEvent((AccountDomainEvent) historicEvent);

                Version++;
            }
        }

        protected override void ApplyEvent(AccountDomainEvent domainEvent)
        {
            Id = domainEvent.AggregateRootId;

            switch (domainEvent)
            {
                case CreditAccountCreatedEvent createdEvent:
                    OnCreditCardAccountCreated(createdEvent);
                    break;
                default:
                    ApplyAccountEvent(domainEvent);
                    break;
            }
        }

        private void OnCreditCardAccountCreated(AccountCreatedEvent cardAccountCreated)
        {
        }
    }
}