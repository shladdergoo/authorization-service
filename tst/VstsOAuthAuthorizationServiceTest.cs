namespace AuthorizationService.Test
{
    using System;
    using System.Net;
    using System.Text;

    using Xunit;
    using NSubstitute;
    using System.IO;

    public class VstsOAuthAuthorizationServiceTest
    {
        IAuthorizationService sut;

        [Fact]
        public void Ctor_NullHttpClient_ThrowsException()
        {
            VstsConfig vstsConfig = new VstsConfig();

            Assert.Throws<ArgumentNullException>(() =>
                this.sut = new VstsOAuthAuthorizationService(null, vstsConfig));
        }

        [Fact]
        public void Ctor_NullVstsConfig_ThrowsException()
        {
            IHttpClient httpClient = Substitute.For<IHttpClient>();

            Assert.Throws<ArgumentNullException>(() =>
                this.sut = new VstsOAuthAuthorizationService(httpClient, null));
        }

        [Fact]
        public void GetOAuthAccessTokens_RequestsTokens()
        {
            IHttpClient httpClient = Substitute.For<IHttpClient>();
            VstsConfig vstsConfig = new VstsConfig
            {
                TokenUrl = @"https://app.vssps.visualstudio.com/oauth2/token",
                AuthorizationUrl = @"http://localhost:5000/api/auth",
                TokenBodyTemplate = "client_assertion_type=urn:ietf:params:oauth:client-assertion-type:jwt-bearer&client_assertion={0}&grant_type=urn:ietf:params:oauth:grant-type:jwt-bearer&assertion={1}&redirect_uri={2}"
            };

            this.sut = new TestableVstsOAuthAuthorizationService(httpClient, vstsConfig);

            this.sut.GetOAuthAccessTokens("someCode", "someState|someSecret");

            httpClient.Received().Execute(Arg.Is<HttpWebRequest>(r => r != null));
        }

        [Fact]
        public void GetOAuthAccessTokens_GetsTokens_ReturnsTokens()
        {
            IHttpClient httpClient = Substitute.For<IHttpClient>();
            HttpWebResponse response = Substitute.For<HttpWebResponse>();
            response.StatusCode.Returns(HttpStatusCode.OK);
            response.GetResponseStream().Returns(this.GetTokensBody());
            httpClient.Execute(Arg.Any<HttpWebRequest>()).Returns(response);

            VstsConfig vstsConfig = new VstsConfig
            {
                TokenUrl = @"https://app.vssps.visualstudio.com/oauth2/token",
                AuthorizationUrl = @"http://localhost:5000/api/auth",
                TokenBodyTemplate = "client_assertion_type=urn:ietf:params:oauth:client-assertion-type:jwt-bearer&client_assertion={0}&grant_type=urn:ietf:params:oauth:grant-type:jwt-bearer&assertion={1}&redirect_uri={2}"
            };

            this.sut = new VstsOAuthAuthorizationService(httpClient, vstsConfig);

            OAuthAccessTokens result = this.sut.GetOAuthAccessTokens("someCode", "someState|someSecret");

            Assert.NotNull(result);
        }

        private Stream GetTokensBody()
        {
            string responseBody =
                "{" +
                    "\"access_token\": \"access token for the user\"," +
                    "\"token_type\": \"type of token\"," +
                    "\"expires_in\": \"3600\"," +
                    "\"refresh_token\": \"refresh token to use to acquire a new access token\"" +
                "}";

            MemoryStream bodyStream = new MemoryStream(Encoding.UTF8.GetBytes(responseBody));
            return bodyStream;
        }
    }
}
