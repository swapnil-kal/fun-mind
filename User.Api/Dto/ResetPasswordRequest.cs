using System.ComponentModel.DataAnnotations;

namespace User.Api.Dto
{
    public class ResetPasswordRequest
    {

        [Required]
        public string Email { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
        
        [Required]
        public string ActiveCode { get; set; }
    }
}
