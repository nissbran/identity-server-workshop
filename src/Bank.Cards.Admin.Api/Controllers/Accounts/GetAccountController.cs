namespace Bank.Cards.Admin.Api.Controllers.Accounts
{
    using System;
    using System.Threading.Tasks;
    using Domain.Account;
    using Microsoft.AspNetCore.Mvc;
    using Models.Accounts;

    [Route("accounts")]
    public class GetAccountController : Controller
    {
        private readonly IAccountDomainRepository _accountDomainRepository;

        public GetAccountController(IAccountDomainRepository accountDomainRepository)
        {
            _accountDomainRepository = accountDomainRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var cardAccount = await _accountDomainRepository.GetAccountById(id);

            if (cardAccount == null)
                return NotFound();

            return Ok(new GetCardAccountResponse(cardAccount));
        }
    }
}