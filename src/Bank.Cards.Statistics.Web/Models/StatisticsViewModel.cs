namespace Bank.Cards.Statistics.Web.Models
{
    using System.Collections.Generic;

    public class StatisticsViewModel
    {
        public IEnumerable<AccountViewModel> Accounts { get; set; }
    }
}