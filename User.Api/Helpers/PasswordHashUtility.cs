using System.Security.Cryptography;
using System.Text;

namespace User.Api.Helpers
{
    public class PasswordHashUtility
    {
        public static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = Convert.ToBase64String(hmac.Key);
                passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        public static bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt)
        {
            byte[] storedHash = Convert.FromBase64String(passwordHash);
            byte[] storedSalt = Convert.FromBase64String(passwordSalt);

            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordSalt");

            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }

        public static string GenerateHashPasswordWithSalt(string value, string salt)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = " ";
            }

            return Convert.ToBase64String(new HMACSHA256(Encoding.UTF8.GetBytes(salt)).ComputeHash(Encoding.UTF8.GetBytes(value)));

        }
    }
}
