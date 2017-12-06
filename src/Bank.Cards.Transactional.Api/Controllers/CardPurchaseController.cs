namespace Bank.Cards.Transactional.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Models.Card;
    using Services.Transactions;

    [Route("cards")]
    public class CardPurchaseController : Controller
    {
        private readonly CreditCardPurchaseService _creditCardPurchaseService;

        public CardPurchaseController(CreditCardPurchaseService creditCardPurchaseService)
        {
            _creditCardPurchaseService = creditCardPurchaseService;
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> Post([FromBody]CardPurchaseRequest request)
        {
            var balance = await _creditCardPurchaseService.CreditCardPurchase(request.Pan, request.Amount);
            
            if (balance == null)
                return NotFound();

            return Ok(new CardPurchaseResponse(balance.Value));
        }
    }
}
