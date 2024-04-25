using System.Security.Claims;

namespace Content.Api.Services
{
    public interface IClaimsProvider
    {
        ClaimsPrincipal UserIdentity { get; }
    }
}
