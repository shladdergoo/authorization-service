namespace AuthorizationService
{
    using System.Net;

    internal interface IHttpClient
    {
        HttpWebResponse Execute(HttpWebRequest webRequest);
    }
}
