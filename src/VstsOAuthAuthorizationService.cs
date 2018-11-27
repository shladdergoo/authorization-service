namespace AuthorizationService
{
    using System;
    using System.IO;
    using System.Net;
    using System.Web;

    using Newtonsoft.Json;

    internal class VstsOAuthAuthorizationService : IAuthorizationService
    {
        private readonly IHttpClient httpClient;
        private readonly VstsConfig vstsConfig;

        public VstsOAuthAuthorizationService(IHttpClient httpClient, VstsConfig vstsConfig)
        {
            if (httpClient == null) { throw new System.ArgumentNullException(nameof(httpClient)); }
            if (vstsConfig == null) { throw new System.ArgumentNullException(nameof(vstsConfig)); }

            this.httpClient = httpClient;
            this.vstsConfig = vstsConfig;
        }

        public OAuthAccessTokens GetOAuthAccessTokens(string code, string state)
        {
            if (code == null) { throw new ArgumentNullException(nameof(code)); }
            if (state == null) { throw new ArgumentNullException(nameof(state)); }

            HttpWebRequest tokenRequest = this.BuildTokenRequest(code);

            HttpWebResponse response = this.httpClient.Execute(tokenRequest);

            return this.GetOAuthTokensFromResponse(response);
        }

        protected virtual OAuthAccessTokens GetOAuthTokensFromResponse(HttpWebResponse response)
        {
            StreamReader streamReader = new StreamReader(response.GetResponseStream());

            OAuthAccessTokens tokens = null;
            using (JsonTextReader textReader = new JsonTextReader(streamReader))
            {
                JsonSerializer serializer = new JsonSerializer();
                tokens = serializer.Deserialize<OAuthAccessTokens>(textReader);
                tokens.Acquired = DateTime.UtcNow;
            }

            return tokens;
        }

        private HttpWebRequest BuildTokenRequest(string authCode)
        {
            Uri tokenRequestUri = new Uri(this.vstsConfig.TokenUrl);

            string tokenRequestData = string.Format(
                    this.vstsConfig.TokenBodyTemplate,
                    HttpUtility.UrlEncode(this.vstsConfig.ClientSecret),
                    HttpUtility.UrlEncode(authCode),
                    this.vstsConfig.AuthorizationUrl);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tokenRequestUri);
            request.Method = "POST";
            request.ContentLength = tokenRequestData.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (StreamWriter streamtWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamtWriter.Write(tokenRequestData);
            }

            return request;
        }
    }
}
