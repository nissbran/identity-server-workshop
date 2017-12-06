namespace Bank.Cards.Statistics.Web.Models
{
    using System;
    using Services.Statistics.Models;

    public class AccountViewModel
    {
        public Guid AccountId { get; }

        public int NumberOfCards { get;  }

        public decimal Balance { get; }

        public AccountViewModel(AccountSummary summary)
        {
            AccountId = summary.AccountId;
            NumberOfCards = summary.NumberOfCards;
            Balance = summary.Balance;
        }
    }
}