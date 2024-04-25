using Content.Api.Constants;
using System.Security.Claims;

namespace Content.Api.Extensions
{
    public static class UserClaimsHelper
    {
        public static int GetRoleId(ClaimsPrincipal user)
        {            
            return Convert.ToInt32(user?.FindFirst(ClaimType.USER_ROLE)?.Value);
        }

        public static string GetName(ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimType.GIVEN_NAME)?.Value;
        }

        public static int GetUserId(ClaimsPrincipal user)
        {
            return Convert.ToInt32(user?.FindFirst(ClaimType.USER_ID)?.Value);
        }

        public static string GetEmailId(ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimType.EMAIL_ADDRESS)?.Value;
        }
    }
}
