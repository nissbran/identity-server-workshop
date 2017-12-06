namespace Bank.Cards.Admin.Web.Configuration
{
    using Infrastructure.Communication;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<AdminHttpService>();
        }
    }
}