using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using IdentityModel.Client;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace AntDesign.Boilerplate.Client.Identity
{
    public class OidcUser
    {
        public OidcUser()
        {
        }

        public OidcUser(TokenResponse token)
        {
            this.AccessToken = token.AccessToken;
            this.IdentityToken = token.IdentityToken;
            this.Scopes = token.Scope;
            this.TokenType = token.TokenType;
            this.RefreshToken = token.RefreshToken;
            this.ExpiresIn = token.ExpiresIn;
        }

        [JsonPropertyName("id_token")]
        public string IdentityToken { get; set; }

        //[JsonPropertyName("session_state")]
        //public string SessionState { get; set; }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("scopes")]
        public string Scopes { get; set; }

        [JsonPropertyName("profile")]
        public ClaimsPrincipal Profile { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        public AccessToken GetAccessToken()
        {
            return new AccessToken
            {
                GrantedScopes = Scopes?.Split(" "),
                Value = AccessToken,
                Expires = DateTime.Now.AddMilliseconds(ExpiresIn)
            };
        }
    }
}