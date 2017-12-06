namespace Bank.Cards.Services.Account.Credit
{
    using System;
    using System.Threading.Tasks;
    using Domain.Account;
    using Domain.Account.Credit;
    using Domain.Account.Credit.Events;
    using Domain.Account.Events;
    using Models;

    public class CreateCreditAccountService
    {
        private readonly IAccountDomainRepository _accountDomainRepository;

        public CreateCreditAccountService(IAccountDomainRepository accountDomainRepository)
        {
            _accountDomainRepository = accountDomainRepository;
        }

        public async Task<Guid> CreateCreditAccount(CreditAccountInfo creditAccountInfo)
        {
            var account = new CreditAccountAggregateRoot(Guid.NewGuid());

            account.AddEvent(new IssuerInformationSetEvent
            {
                IssuerId = creditAccountInfo.IssuerId
            });
            account.AddEvent(new CreditLimitSetEvent
            {
                CreditLimit = creditAccountInfo.CreditLimit
            });

            await _accountDomainRepository.SaveAccount(account);

            return account.Id;
        }
    }
}