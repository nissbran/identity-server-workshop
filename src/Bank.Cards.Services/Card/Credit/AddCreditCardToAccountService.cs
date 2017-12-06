namespace Bank.Cards.Services.Card.Credit
{
    using System;
    using System.Threading.Tasks;
    using Domain.Account;
    using Domain.Card;
    using Domain.Card.Credit;
    using Domain.Card.Credit.Events;
    using Domain.Card.Events;
    using Models;
    using Security;

    public class AddCreditCardToAccountService
    {
        private readonly IAccountDomainRepository _accountDomainRepository;
        private readonly ICardDomainRepository _cardDomainRepository;

        private readonly CardPanGeneratorService _cardPanGeneratorService;
        private readonly PanEncryptService _panEncryptService;
        private readonly PanHashService _panHashService;

        public AddCreditCardToAccountService(IAccountDomainRepository accountDomainRepository, ICardDomainRepository cardDomainRepository)
        {
            _accountDomainRepository = accountDomainRepository;
            _cardDomainRepository = cardDomainRepository;

            _cardPanGeneratorService = new CardPanGeneratorService();
            _panEncryptService = new PanEncryptService();
            _panHashService = new PanHashService();
        }

        public async Task<CreditCardInfo> AddCreditCardToAccount(Guid id, CreditCardInfo creditCardInfo)
        {
            var cardAccount = await _accountDomainRepository.GetAccountById(id);

            if (cardAccount == null)
                return null;

            var newPan = _cardPanGeneratorService.GeneratePan();
            var hashedPan = _panHashService.HashPan(newPan);

            var newCard = new CreditCardAggregateRoot(hashedPan);

            newCard.AddEvent(new CreditCardCreatedEvent
            {
                HashedPan = hashedPan,
                EncryptedPan = _panEncryptService.EncryptPan(newPan)
            });
            newCard.AddEvent(new CardDetailsSetEvent
            {
                NameOnCard = creditCardInfo.NameOnCard
            });
            newCard.AddEvent(new CardConnectedToAccountEvent
            {
                AccountId = cardAccount.Id
            });

            await _cardDomainRepository.SaveCard(newCard);

            return new CreditCardInfo
            {
                NameOnCard = creditCardInfo.NameOnCard,
                Pan = newPan
            };
        }
    }
}