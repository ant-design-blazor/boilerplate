using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AntDesign.Boilerplate.Client.Identity;
using AntDesign.Boilerplate.Client.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;

namespace AntDesign.Boilerplate.Client.Services
{
    public interface IAccountService
    {
        Task<bool> LoginAsync(LoginParamsType model);

        Task<bool> IsLoginAsync();

        Task<bool> LogoutAsync();

        Task<string> GetCaptchaAsync(string modile);
    }

    public class AccountService : IAccountService
    {
        private readonly Random _random = new Random();

        public HttpClient _httpClient;

        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private AccessTokenService _accessTokenService;
        private RemoteAuthenticationOptions<OidcProviderOptions> _options;
        private SignOutSessionStateManager _signOutManager;

        public AccountService(HttpClient httpClient,
            AuthenticationStateProvider authenticationStateProvider,
            AccessTokenService accessTokenService,
            SignOutSessionStateManager signOutManager,
            IOptions<RemoteAuthenticationOptions<OidcProviderOptions>> options
            )
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _accessTokenService = accessTokenService;
            _signOutManager = signOutManager;
            _options = options.Value;
        }

        public async Task<bool> LoginAsync(LoginParamsType model)
        {
            var document = await _httpClient.GetDiscoveryDocumentAsync();
            var tokenResult = await _httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = document.TokenEndpoint,
                UserName = model.UserName,
                Password = model.Password,
                ClientId = _options.ProviderOptions.ClientId,
                Scope = string.Join(" ", _options.ProviderOptions.DefaultScopes)
            });

            if (!tokenResult.IsError)
            {
                await _accessTokenService.SaveAccessToken(tokenResult);
                ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(model.UserName);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tokenResult.AccessToken);

                return true;
            }

            return false;
        }

        public Task<string> GetCaptchaAsync(string modile)
        {
            var captcha = _random.Next(0, 9999).ToString().PadLeft(4, '0');
            return Task.FromResult(captcha);
        }

        public async Task<bool> LogoutAsync()
        {
            await _accessTokenService.ClearToken();
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
            await _signOutManager.SetSignOutState();
            return true;
        }

        public async Task<bool> IsLoginAsync()
        {
            return (await _accessTokenService.GetAccessToken()) != null;
        }
    }
}