using User.Api.Dto;

namespace User.Api.Services
{
    public interface IJwtAuthProvider
    {
        string GenerateAccessToken(UserDto user, List<string> allowedPermissions);

        string GenerateRefreshToken();
    }
}
