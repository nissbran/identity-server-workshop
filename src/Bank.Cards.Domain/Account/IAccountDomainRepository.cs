namespace Bank.Cards.Domain.Account
{
    using System;
    using System.Threading.Tasks;

    public interface IAccountDomainRepository
    {
        Task<AccountAggregateRoot> GetAccountById(Guid cardId);

        Task SaveAccount(AccountAggregateRoot account);
    }
}