using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using User.Api.Constants;
using User.Api.Dto;
using User.Api.Helpers;

namespace User.Api.Services
{
    public class JwtAuthProvider : IJwtAuthProvider
    {
        private readonly JwtTokenConfiguration jwtTokenConfiguration;

        public JwtAuthProvider(JwtTokenConfiguration _jwtTokenConfiguration)
        {
            jwtTokenConfiguration = _jwtTokenConfiguration;
        }

        public string GenerateAccessToken(UserDto user, List<string> allowedPermissions)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimType.USER_ID , Convert.ToString(user.Id)),
                new Claim(ClaimType.EMAIL_ADDRESS, user.Email),
                new Claim(ClaimType.GIVEN_NAME, string.Format("{0} {1}", user.FirstName, user.LastName)),
                new Claim(ClaimType.USER_ROLE, Convert.ToString(user.RoleId)),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim(ClaimType.AGE_GROUP_ID,  user.UserProfile != null ? Convert.ToString(user.UserProfile.AgeGroupId) : null),
                new Claim(ClaimType.CATEGORY_IDS, user.UserProfile != null ? Convert.ToString(string.Join(",", user.UserProfile.CategoryIds)) : null),
            };

            foreach (var permission in allowedPermissions)
            {
                claims.Add(new Claim("permissions", permission));
            }

            var jwtToken = new JwtSecurityToken(
                jwtTokenConfiguration.Issuer,
                jwtTokenConfiguration.Audience,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(jwtTokenConfiguration.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfiguration.Key)), SecurityAlgorithms.HmacSha256Signature));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return accessToken;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
