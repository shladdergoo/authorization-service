namespace AuthorizationService
{
    public interface IAuthorizationService
    {
        OAuthAccessTokens GetOAuthAccessTokens(string code, string state);
    }
}
