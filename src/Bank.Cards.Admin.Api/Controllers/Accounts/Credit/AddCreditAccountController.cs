namespace Bank.Cards.Admin.Api.Controllers.Accounts.Credit
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Models.Accounts;
    using Services.Account.Credit;
    using Services.Models;

    [Route("accounts/credit")]
    public class AddCreditAccountController : Controller
    {
        private readonly CreateCreditAccountService _createCreditAccountService;

        public AddCreditAccountController(CreateCreditAccountService createCreditAccountService)
        {
            _createCreditAccountService = createCreditAccountService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddCreditCardAccountRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var accountId = await _createCreditAccountService.CreateCreditAccount(new CreditAccountInfo
            {
                IssuerId = request.IssuerId.Value,
                CreditLimit = request.CreditLimit
            });

            return Ok(new AddAccountResponse
            {
                AccountId = accountId
            });
        }
    }
}