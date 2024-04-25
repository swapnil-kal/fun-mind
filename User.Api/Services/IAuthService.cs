using User.Api.Dto;

namespace User.Api.Services
{
    public interface IAuthService
    {
        Task<RegisterResponse> Register(RegisterRequest request);

        Task<LoginResponse> Login(LoginRequest request);

        Task<LoginResponse> ExternalLogin(ExternalAuthDto request);

        Task<LoginResponse> GuestLogin(LoginRequest request);

        Task<LoginResponse> Refresh(LoginRequest request);

        Task<bool> ChangePassword(ChangePasswordRequest request);

        Task<bool> SendResetPasswordOTP(string userEmail);

        Task<bool> VerifyResetPasswordOTP(string userEmail, string activeCode);

        Task<bool> ResetPassword(ResetPasswordRequest request);

        Task<bool> Logout();
    }
}
