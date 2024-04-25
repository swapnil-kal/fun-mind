using System.Security.Claims;
using User.Api.Constants;

namespace User.Api.Extensions
{
    public static class UserClaimsHelper
    {
        public static int GetRoleId(ClaimsPrincipal user)
        {
            var roleId = user?.FindFirst(ClaimType.USER_ROLE)?.Value;
            return Convert.ToInt32(roleId);
        }

        public static string GetName(ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimType.GIVEN_NAME)?.Value;
        }

        public static int GetUserId(ClaimsPrincipal user)
        {
            var userId = user?.FindFirst(ClaimType.USER_ID)?.Value;
            return Convert.ToInt32(userId);
        }

        public static string GetEmailId(ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimType.EMAIL_ADDRESS)?.Value;
        }
    }
}
