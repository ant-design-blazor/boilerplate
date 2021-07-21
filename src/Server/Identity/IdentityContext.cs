using AntDesign.Boilerplate.Shared;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AntDesign.Boilerplate.Server.Identity
{
    public class IdentityContext : IIdentityContext
    {
        public static readonly IdentityContext Empty = new IdentityContext();
        private readonly ClaimsPrincipal User;

        public IdentityContext()
        {
        }

        public IdentityContext(IHttpContextAccessor httpContextAccessor)
        {
            User = httpContextAccessor.HttpContext?.User;
        }

        public virtual long? UserId => this.GetClaimValueAsLong(User, ClaimTypes.NameIdentifier);

        public virtual string UserName => this.GetClaimValue(User, JwtClaimTypes.Name);

        public string NickName => this.GetClaimValue(User, JwtClaimTypes.NickName);

        public string Avatar => this.GetClaimValue(User, JwtClaimTypes.Picture);

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

        public Task GetState()
        {
            return Task.CompletedTask;
        }
    }
}