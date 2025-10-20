using Microsoft.Extensions.DependencyInjection;

namespace Finomal.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);
            });
            //services.AddScoped<IArticlesViewService, ArticlesViewService>();
            return services;
        }
    }
}
