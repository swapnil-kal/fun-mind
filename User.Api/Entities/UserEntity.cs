using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User.Api.Entities
{
    [Table("Users")]
    public class UserEntity : BaseEntity
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        public int RoleId { get; set; }

        public string Address { get; set; }

        public string ProfileImageUrl { get; set; }

        public string ProfileImageName { get; set; }

        public string RefreshToken { get; set; }

        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }

        public string ActiveCode { get; set; }

        public DateTimeOffset? ActiveCodeExpireOn { get; set; }

        public virtual RoleEntity Role { get; set; }

        public virtual UserProfileEntity UserProfile { get; set; }

    }
}
