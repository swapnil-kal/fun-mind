using System.ComponentModel.DataAnnotations;

namespace User.Api.Dto
{
    public class VerifyResetPasswordOTPRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Otp { get; set; }
    }
}
