namespace Bank.Cards.Admin.Web.Models
{
    public class NewCreditAccountRequest
    {
        public int IssuerId { get; set; }

        public decimal CreditLimit { get; set; }
    }
}