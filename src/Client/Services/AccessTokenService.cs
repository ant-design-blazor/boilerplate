using AntDesign.Boilerplate.Client.Identity;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace AntDesign.Boilerplate.Client.Services
{
    public class AccessTokenService
    {
        private LocalStorage _localStorage;
        private RemoteAuthenticationOptions<OidcProviderOptions> _options;

        private string StoreKey => $"user:{_options.ProviderOptions.Authority}:{_options.ProviderOptions.ClientId}";

        public AccessTokenService(LocalStorage localStorage, IOptions<RemoteAuthenticationOptions<OidcProviderOptions>> options)
        {
            _localStorage = localStorage;
            _options = options.Value;
        }

        public async ValueTask<AccessToken> GetAccessToken()
        {
            var userFormStorage = await _localStorage.GetAsync<OidcUser>(StoreKey);
            return userFormStorage?.GetAccessToken();
        }

        public ValueTask SaveAccessToken(TokenResponse token)
        {
            return _localStorage.SetAsync(StoreKey, new OidcUser(token));
        }

        public ValueTask ClearToken()
        {
            return _localStorage.Clear(StoreKey);
        }
    }
}