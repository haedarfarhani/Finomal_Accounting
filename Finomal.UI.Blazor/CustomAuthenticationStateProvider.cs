using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Finomal.UI.Blazor
{
    public class CustomAuthenticationStateProvider : ServerAuthenticationStateProvider
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;

        public CustomAuthenticationStateProvider(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var httpContext = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;

            if (httpContext?.User?.Identity?.IsAuthenticated == true)
            {
                return await base.GetAuthenticationStateAsync(); 
            }

            var token = httpContext?.Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // اعتبارسنجی JWT
            var principal = ValidateToken(token);
            if (principal != null)
            {
                var authState = new AuthenticationState(principal);
                NotifyAuthenticationStateChanged(Task.FromResult(authState));
                return authState;
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        private ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "YourSecretKeyHere"); // کلید JWT از appsettings.json

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = jwtToken.Claims;
                var identity = new ClaimsIdentity(claims, "jwt");
                return new ClaimsPrincipal(identity);
            }
            catch
            {
                return null; // توکن نامعتبر
            }
        }
    }
}
