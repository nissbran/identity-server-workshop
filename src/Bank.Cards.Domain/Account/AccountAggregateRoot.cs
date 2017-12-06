namespace Bank.Cards.Domain.Account
{
    using System;
    using Events;
    using Events.Credit;
    using Events.Debit;
    using Infrastructure.Domain;
    using State;

    public abstract class AccountAggregateRoot : AggregateRoot
    {
        public Guid Id { get; protected set; }
        
        public AccountState State { get; protected set; }

        public void AddEvent(AccountDomainEvent domainEvent)
        {
            domainEvent.AggregateRootId = Id;

            ApplyEvent(domainEvent);

            UncommittedEvents.Add(domainEvent);

            Version++;
        }

        protected abstract void ApplyEvent(AccountDomainEvent domainEvent);

        protected void ApplyAccountEvent(AccountDomainEvent domainEvent)
        {
            Id = domainEvent.AggregateRootId;

            switch (domainEvent)
            {
                case AccountDebitedEvent accountDebitedEvent:
                    OnAccountDebited(accountDebitedEvent);
                    break;
                case AccountCreditedEvent accountCreditedEvent:
                    OnAccountCredited(accountCreditedEvent);
                    break;
                case IssuerInformationSetEvent issuerInformationSetEvent:
                    OnIssuerInformationSetEvent(issuerInformationSetEvent);
                    break;
            }
        }

        private void OnAccountDebited(AccountDebitedEvent accountDebitedEvent)
        {
            State.Balance -= accountDebitedEvent.Amount;
        }

        private void OnAccountCredited(AccountCreditedEvent accountCreditedEvent)
        {
            State.Balance += accountCreditedEvent.Amount;
        }

        private void OnIssuerInformationSetEvent(IssuerInformationSetEvent issuerInformationSetEvent)
        {
            State.IssuerId = issuerInformationSetEvent.IssuerId;
        }
    }
}