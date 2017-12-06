namespace Bank.Cards.Domain.Card.Events
{
    using Infrastructure.Domain;

    [EventName("CardDetailsSet")]
    public class CardDetailsSetEvent : CardDomainEvent
    {
        public string NameOnCard { get; set; }
    }
}