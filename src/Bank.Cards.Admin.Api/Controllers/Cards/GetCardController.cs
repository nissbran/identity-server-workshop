namespace Bank.Cards.Admin.Api.Controllers.Cards
{
    using System.Threading.Tasks;
    using Domain.Card;
    using Microsoft.AspNetCore.Mvc;
    using Models.Cards;
    using Services.Security;

    [Route("cards")]
    public class GetCardController : Controller
    {
        private readonly ICardDomainRepository _cardDomainRepository;
        
        private readonly PanHashService _panHashService;

        public GetCardController(ICardDomainRepository cardDomainRepository)
        {
            _cardDomainRepository = cardDomainRepository;
            
            _panHashService = new PanHashService();
        }

        [HttpGet("{pan}")]
        public async Task<IActionResult> GetCardByPan(string pan)
        {
            var hashedPan = _panHashService.HashPan(pan);
            var card = await _cardDomainRepository.GetCardByHashedPan(hashedPan);

            if (card == null)
                return NotFound();

            return Ok(new GetCardResponse(card));
        }
    }
}