using System.ComponentModel.DataAnnotations;

namespace User.Api.Dto
{
    public class CreateUserRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }        

        [Required]
        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}
