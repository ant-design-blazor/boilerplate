using AntDesign.Boilerplate.Shared;
using IdentityModel;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AntDesign.Boilerplate.Client.Identity
{
    public class IdentityContext : IIdentityContext, IDisposable
    {
        private AuthenticationStateProvider _authenticationStateProvider;
        private ClaimsPrincipal _claimsPrincipal;

        public IdentityContext(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _authenticationStateProvider.AuthenticationStateChanged += AuthenticationStateChanged;
        }

        private async void AuthenticationStateChanged(Task<AuthenticationState> task) => await GetState();

        public async Task GetState()
        {
            var userState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            _claimsPrincipal = userState.User;
        }

        public virtual long? UserId => this.GetClaimValueAsLong(_claimsPrincipal, JwtClaimTypes.Subject);

        public virtual string UserName => this.GetClaimValue(_claimsPrincipal, JwtClaimTypes.Name);

        public string NickName => this.GetClaimValue(_claimsPrincipal, JwtClaimTypes.NickName);

        public string Avatar => this.GetClaimValue(_claimsPrincipal, JwtClaimTypes.Picture);

        private string GetClaimValue(ClaimsPrincipal user, string claimType)
        {
            var first = user?.FindFirst(claimType);
            return first?.Value;
        }

        private long? GetClaimValueAsLong(ClaimsPrincipal user, string claimType)
        {
            var claimValue = this.GetClaimValue(user, claimType);
            if (claimValue == null)
            {
                return null;
            }

            if (!long.TryParse(claimValue, out var value))
            {
                return null;
            }

            return value;
        }

        public void Dispose()
        {
            _authenticationStateProvider.AuthenticationStateChanged -= AuthenticationStateChanged;
        }
    }
}