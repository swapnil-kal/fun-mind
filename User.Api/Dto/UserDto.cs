using User.Api.Entities;

namespace User.Api.Dto
{
    public class UserDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string GivenName
        {
            get { return $"{FirstName} {LastName}"; }
        }
        
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        public int RoleId { get; set; }

        public string Address { get; set; }

        public RoleDto Role { get; set; }

        public UserProfileDto UserProfile { get; set; }

        public int? CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public int? DeletedBy { get; set; }

        public DateTimeOffset? Deleted { get; set; }

    }
}
