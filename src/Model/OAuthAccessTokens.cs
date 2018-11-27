namespace AuthorizationService
{
    using System;

    using Newtonsoft.Json;

    public class OAuthAccessTokens
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; internal set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; internal set; }

        [JsonProperty("token_type")]
        public string TokenType { get; internal set; }

        [JsonProperty("acquired")]
        public DateTime Acquired { get; internal set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; internal set; }
    }
}
