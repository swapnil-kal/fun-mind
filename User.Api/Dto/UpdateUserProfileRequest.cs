using System.ComponentModel.DataAnnotations;

namespace User.Api.Dto
{
    public class UpdateUserProfileRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        
        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        public int AgeGroupId { get; set; }

        public List<int> CategoryIds { get; set; }

    }
}
