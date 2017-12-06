namespace Bank.Cards.Admin.Web.Controllers
{
    using System.Threading.Tasks;
    using Infrastructure.Communication;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Models.Admin;
    using Newtonsoft.Json;
    
    public class AccountController : Controller
    {
        private readonly AdminHttpService _adminHttpService;

        public AccountController(AdminHttpService adminHttpService)
        {
            _adminHttpService = adminHttpService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(NewCreditAccountRequest request)
        {
            var backendRequest = new AddCreditCardAccountRequest
            {
                IssuerId = request.IssuerId,
                CreditLimit = request.CreditLimit
            };

            var response = await _adminHttpService.PostAsync("accounts/credit", JsonConvert.SerializeObject(backendRequest));

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return BadRequest();
        }
    }
}