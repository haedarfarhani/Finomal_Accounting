using Finomal.Application.Accounting;
using Finomal.Application.Users;
using Finomal.Domain.Accounting;
using Finomal.Domain.Users;
using Finomal.Infrastructure.Data.Accounting;
using Finomal.Infrastructure.Data.Context;
using Finomal.Infrastructure.Data.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finomal.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountingRepository, AccountingRepository>();
            services.AddScoped<IAccountingService, AccountingService>();
            return services;
        }
    }
}
