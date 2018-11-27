namespace AuthorizationService.Test
{
    using System.Net;

    internal class TestableVstsOAuthAuthorizationService : VstsOAuthAuthorizationService
    {
        public TestableVstsOAuthAuthorizationService(IHttpClient httpClient, VstsConfig vstsConfig) : base(httpClient, vstsConfig)
        {
        }

        protected override OAuthAccessTokens GetOAuthTokensFromResponse(HttpWebResponse response)
        {
            return null;
        }
    }
}
