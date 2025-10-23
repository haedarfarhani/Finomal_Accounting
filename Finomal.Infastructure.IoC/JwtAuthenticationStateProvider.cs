using Finomal.Infrastructure.Data.Users; 
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Finomal.Infastructure.IoC
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        private readonly ILogger<JwtAuthenticationStateProvider> _logger;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity()); 

        public JwtAuthenticationStateProvider(
            ProtectedLocalStorage protectedLocalStorage,
            ILogger<JwtAuthenticationStateProvider> logger,
            IOptions<JwtSettings> jwtOptions)
        {
            _protectedLocalStorage = protectedLocalStorage;
            _logger = logger;
            _tokenValidationParameters = CreateTokenValidationParameters(jwtOptions.Value);
        }

        private TokenValidationParameters CreateTokenValidationParameters(JwtSettings jwtSettings)
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                ClockSkew = TimeSpan.Zero
            };
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            
            try
            {
                var result = await _protectedLocalStorage.GetAsync<string>("authToken");
                if (!result.Success || string.IsNullOrEmpty(result.Value))
                {
                    return new AuthenticationState(_anonymous);
                }

                var token = result.Value;
                var handler = new JwtSecurityTokenHandler();

                if (!handler.CanReadToken(token))
                {
                    _logger.LogWarning("Cannot read JWT token. Removing invalid token.");
                    await RemoveTokenAsync(false);
                    return new AuthenticationState(_anonymous);
                }

                try
                {
                    var principal = handler.ValidateToken(token, _tokenValidationParameters, out SecurityToken validatedToken);
                    if (validatedToken == null || IsTokenExpired(validatedToken))
                    {
                        _logger.LogWarning("JWT token is expired or invalid. Removing token.");
                        await RemoveTokenAsync(false);
                        return new AuthenticationState(_anonymous);
                    }
                    return new AuthenticationState(principal);
                }
                catch (SecurityTokenValidationException stvex)
                {
                    _logger.LogError(stvex, "JWT token validation failed. Removing token.");
                    await RemoveTokenAsync(false);
                    return new AuthenticationState(_anonymous);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error validating JWT from ProtectedLocalStorage after read. Removing token.");
                    await RemoveTokenAsync(false);
                    return new AuthenticationState(_anonymous);
                }
            }
            catch (JSException jsEx) when (jsEx.Message.Contains("JavaScript interop calls cannot be issued"))
            {
                _logger.LogWarning("JS Interop failed during prerendering in GetAuthenticationStateAsync. Returning anonymous state. A client-side refresh will attempt to resolve this.");
                return new AuthenticationState(_anonymous);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error in GetAuthenticationStateAsync.");
                return new AuthenticationState(_anonymous);
            }
        }

        private bool IsTokenExpired(SecurityToken token)
        {
            return token.ValidTo < DateTime.UtcNow;
        }

        public async Task SetToken(string token)
        {
            try
            {
                await _protectedLocalStorage.SetAsync("authToken", token);
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
            catch (JSException jsEx) when (jsEx.Message.Contains("JavaScript interop calls cannot be issued"))
            {
                _logger.LogWarning("Failed to set token during prerendering. Will attempt to set after render.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting token.");
            }
        }

        public async Task RemoveTokenAsync(bool notify = true)
        {
            try
            {
                await _protectedLocalStorage.DeleteAsync("authToken");
                if (notify)
                {
                    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
                }
            }
            catch (JSException jsEx) when (jsEx.Message.Contains("JavaScript interop calls cannot be issued"))
            {
                _logger.LogWarning("Failed to remove token during prerendering. Will attempt to remove after render.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing token.");
            }
        }

        public async Task LogoutAsync()
        {
            await RemoveTokenAsync(true);
        }

        public void RefreshAuthenticationStateAsync()
        {
            _logger.LogInformation("Forcing authentication state refresh.");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}