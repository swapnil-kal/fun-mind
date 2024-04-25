using System.ComponentModel.DataAnnotations;

namespace User.Api.Dto
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public int AgeGroupId { get; set; }

        [Required]
        public List<int> CategoryIds { get; set; }
    }
}
