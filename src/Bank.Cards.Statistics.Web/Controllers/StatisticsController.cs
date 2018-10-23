namespace Bank.Cards.Statistics.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services.Statistics;
    
    [Authorize]
    public class StatisticsController : Controller
    {
        private readonly GetAccountStatisticsService _accountStatisticsService;

        public StatisticsController(GetAccountStatisticsService accountStatisticsService)
        {
            _accountStatisticsService = accountStatisticsService;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _accountStatisticsService.GetAccountSummary();

            return View(new StatisticsViewModel
            {
                Accounts = accounts.Select(summary => new AccountViewModel(summary))
            });
        }
    }
}