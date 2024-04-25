using System.ComponentModel.DataAnnotations;

namespace User.Api.Dto
{
    public class UpdateUserRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
    }
}
