namespace Bank.Cards.Admin.Api.Models.Accounts
{
    using System.ComponentModel.DataAnnotations;

    public class AddCreditCardAccountRequest
    { 
        [Required]
        public long? IssuerId { get; set; }

        public decimal CreditLimit { get; set; }
    }
}