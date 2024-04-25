using System.Security.Claims;

namespace User.Api.Services
{
    public class HttpContextClaimsProvider : IClaimsProvider
    {
        private readonly IHttpContextAccessor _http;
        public HttpContextClaimsProvider(IHttpContextAccessor http)
        {
            _http = http;
        }

        public ClaimsPrincipal UserIdentity {
            get { return _http.HttpContext.User; }
        }

        public string GetCurrentUser()
        {
            return _http.HttpContext.User.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        }
    }
}
