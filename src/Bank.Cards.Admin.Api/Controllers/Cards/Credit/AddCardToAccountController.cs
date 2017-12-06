namespace Bank.Cards.Admin.Api.Controllers.Cards.Credit
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Models.Cards;
    using Services.Card.Credit;
    using Services.Models;

    [Route("accounts")]
    public class AddCardToAccountController : Controller
    {
        private readonly AddCreditCardToAccountService _addCreditCardToAccountService;

        public AddCardToAccountController(AddCreditCardToAccountService addCreditCardToAccountService)
        {
            _addCreditCardToAccountService = addCreditCardToAccountService;
        }
        
        [HttpPost("{id}/creditcards")]
        public async Task<IActionResult> AddCreditCardToAccount(Guid id, [FromBody]AddCreditCardToAccountRequest request)
        {
            var result = await _addCreditCardToAccountService.AddCreditCardToAccount(id, new CreditCardInfo
            {
                NameOnCard = request.NameOnCard
            });
            
            if (result == null)
                return NotFound();
            
            return Ok(new AddCardToAccountResponse
            {
                Pan = result.Pan
            });
        }
    }
}