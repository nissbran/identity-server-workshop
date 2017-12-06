namespace Bank.Cards.Services.Transactions
{
    using System.Threading.Tasks;
    using Domain.Account;
    using Domain.Account.Events.Debit;
    using Domain.Card;
    using Security;

    public class CreditCardPurchaseService
    {
        private readonly IAccountDomainRepository _accountDomainRepository;
        private readonly ICardDomainRepository _cardDomainRepository;

        private readonly PanHashService _panHashService;

        public CreditCardPurchaseService(IAccountDomainRepository accountDomainRepository, ICardDomainRepository cardDomainRepository)
        {
            _accountDomainRepository = accountDomainRepository;
            _cardDomainRepository = cardDomainRepository;

            _panHashService = new PanHashService();
        }

        public async Task<decimal?> CreditCardPurchase(string pan, decimal amount)
        {
            var hashedPan = _panHashService.HashPan(pan);
            var card = await _cardDomainRepository.GetCardByHashedPan(hashedPan);

            if (card == null)
                return null;

            var account = await _accountDomainRepository.GetAccountById(card.State.AccountId);

            account.AddEvent(new AccountDebitedEvent
            {
                Amount = amount
            });

            await _accountDomainRepository.SaveAccount(account);

            return account.State.Balance;
        }
    }
}