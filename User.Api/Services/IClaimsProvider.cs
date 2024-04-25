using System.Security.Claims;

namespace User.Api.Services
{
    public interface IClaimsProvider
    {
        ClaimsPrincipal UserIdentity { get; }
    }
}
