using System.ComponentModel.DataAnnotations;

namespace User.Api.Dto
{
    public class ResetPasswordOTPRequest
    {
        [Required]
        public string Email { get; set; }
    }
}
