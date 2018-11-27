namespace AuthorizationService
{
    using System;
    using System.Text;

    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;

    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthorizationService authorizationService;

        public AuthController(IAuthorizationService authorizationService)
        {
            if (authorizationService == null) { throw new ArgumentNullException(nameof(authorizationService)); }

            this.authorizationService = authorizationService;
        }

        [HttpGet]
        public ActionResult Get(string code, string state)
        {
            if (code == null) { throw new ArgumentNullException(nameof(code)); }
            if (state == null) { throw new ArgumentNullException(nameof(state)); }

            return this.GetFileResult(this.authorizationService.GetOAuthAccessTokens(code, state));
        }

        private FileResult GetFileResult(OAuthAccessTokens oAuthAccessTokens)
        {
            string tokenString = JsonConvert.SerializeObject(oAuthAccessTokens);
            byte[] tokenByt = Encoding.UTF8.GetBytes(tokenString);

            return this.File(tokenByt, "application/json", "tokens.json");
        }
    }
}
