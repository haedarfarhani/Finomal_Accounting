using BlazorCleanArchitecture.Infrastructure.Authentication;
using Blazored.LocalStorage;
using Finomal.Application.Accounting;
using Finomal.Application.Users;
using Finomal.Domain.Accounting;
using Finomal.Domain.Users;
using Finomal.Infrastructure.Data.Accounting;
using Finomal.Infrastructure.Data.Context;
using Finomal.Infrastructure.Data.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Finomal.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddBlazoredLocalStorage();

            services.AddSingleton<IOptions<JwtSettings>>(provider =>
                Options.Create(configuration.GetSection("JwtSettings").Get<JwtSettings>()));

            // Add Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
                    if (string.IsNullOrEmpty(jwtSettings.Key))
                    {
                        throw new InvalidOperationException("JWT Key cannot be null or empty.");
                    }

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                });



            AddAuthentication(services);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAccountingRepository, AccountingRepository>();

            services.AddScoped<IAccountingService, AccountingService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }

        private static void AddAuthentication(IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
            services.AddCascadingAuthenticationState();
            services.AddAuthorization();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }).AddIdentityCookies();
            services.AddIdentityCore<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
            })
                .AddRoles<Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();
        }
    }
}
