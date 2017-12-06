namespace Bank.Cards.Admin.Api.Models.Accounts
{
    using Services.Issuers;

    public class IssuerReponseItem
    {
        public long IssuerId { get; }

        public string IssuerName { get; }

        public IssuerReponseItem(long issuerId)
        {
            var issuer = IssuerConfiguration.Issuers[issuerId];

            IssuerId = issuer.Id;
            IssuerName = issuer.Name;
        }
    }
}