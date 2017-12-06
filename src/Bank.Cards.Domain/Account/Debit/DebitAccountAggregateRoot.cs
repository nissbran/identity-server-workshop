namespace Bank.Cards.Domain.Account.Debit
{
    using System;
    using System.Collections.Generic;
    using Account.Events;
    using Events;
    using Infrastructure.Domain;
    using State;

    public sealed class DebitAccountAggregateRoot : AccountAggregateRoot
    {
        private DebitAccountAggregateRoot()
        {
            State = new DebitAccountState();
        }

        public DebitAccountAggregateRoot(Guid id) : this()
        {
            Id = id;

            AddEvent(new DebitAccountCreatedEvent());
        }

        public DebitAccountAggregateRoot(IEnumerable<IDomainEvent> historicEvents) : this()
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
                case DebitAccountCreatedEvent createdEvent:
                    OnDebitCardAccountCreated(createdEvent);
                    break;
                default:
                    ApplyAccountEvent(domainEvent);
                    break;
            }
        }

        private void OnDebitCardAccountCreated(AccountCreatedEvent cardAccountCreated)
        {
        }
    }
}