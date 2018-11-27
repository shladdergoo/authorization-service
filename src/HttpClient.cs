namespace AuthorizationService
{
    using System.Net;

    internal class HttpClient : IHttpClient
    {
        public HttpWebResponse Execute(HttpWebRequest webRequest)
        {
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

            return response;
        }
    }
}
