namespace Bank.Cards.Admin.Api.Configuration
{
    using System.Collections.Generic;
    using Bank.Infrastructure.Configuration;
    using Bank.Infrastructure.EventStore;
    using Domain.Account;
    using Domain.Card;
    using Domain.Schemas;
    using Infrastructure.Logging;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Services.Account.Credit;
    using Services.Card.Credit;

    public static class ApplicationServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IEventStore, EventStore>(provider =>
            {
                var connection = EventStoreConnectionFactory.Create(
                    new EventStoreSingleNodeConfiguration(),
                    new EventStoreLogger(provider.GetRequiredService<ILoggerFactory>()),
                    "admin", "changeit");

                connection.ConnectAsync().Wait();

                var schemas = new List<IEventSchema>
                {
                    new AccountSchema(),
                    new CardSchema()
                };

                return new EventStore(connection, schemas);
            });

            services.AddSingleton<IAccountDomainRepository, AccountDomainRepository>();
            services.AddSingleton<ICardDomainRepository, CardDomainRepository>();

            services.AddTransient<AddCreditCardToAccountService>();
            services.AddTransient<CreateCreditAccountService>();
        }
    }
}