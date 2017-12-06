namespace Bank.Cards.Admin.Web.Models.Admin
{
    public class AddCreditCardAccountRequest
    {
        public long IssuerId { get; set; }

        public decimal CreditLimit { get; set; }
    }
}